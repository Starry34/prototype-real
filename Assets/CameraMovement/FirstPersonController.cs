using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove {  get; private set; } = true;

    public bool playerIsAlive = true;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey) && characterController.isGrounded;
    private bool shouldJump => Input.GetKey(jumpKey);
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;
    private bool shouldPause => Input.GetKey(Pause);


    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadBob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;


    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode Pause = KeyCode.P;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 2.0f;


    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedx = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedy = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 3.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Crouch Parameters")]
    //crouch height
    [SerializeField] private float crouchHeight = 0.5f;
    //stand height
    [SerializeField] private float standingHeight = 2.0f;

    [SerializeField] private float timeToCrouch = 0.25f;
    //time to crouch/stand
    //standing center point
    [SerializeField] Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    //crouchig center point
    [SerializeField] Vector3 standingCenter = new Vector3(0,0,0);

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;

    [Header("Coyote Time & jump buffer")]
    [SerializeField] private float CoyoteTime = 0.4f;
    [SerializeField] private float CoyoteTimeCounter = 0.0f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferCounter;

     
    [Header("Jumping boots Parameters")]
    [SerializeField] private static bool JumpBoots = false;
    [SerializeField] private float BootForce = 7.5f;
    [SerializeField] private float walkBootSpeed = 4.5f;
    [SerializeField] private float sprintBootSpeed = 9.0f;
    [SerializeField] private float crouchBootSpeed = 2.5f;

    [Header("Jet Pack Parameters")]
    [SerializeField] private bool jetPack = false;

    [Header("Key paramateres")]
    [SerializeField] public static int keyValue = 0;
    [SerializeField] private GameObject key;

    [Header("Pause parameters")]
    public static bool gameIsPaused;
    [SerializeField]public GameObject pauseLabel;

    [Header("Sound Parameters")]
    [SerializeField] public AudioSource walkingSound;
    [SerializeField] public AudioSource RunningSound;
    [SerializeField] public AudioSource JumpingSound;
    [SerializeField] public AudioSource JetPackSound;
    [SerializeField] public AudioSource JetPackCollectSound;
    [SerializeField] public AudioSource CashCollectSound;
    [SerializeField] public AudioSource BootCollectSound;
    [SerializeField] public AudioSource KeyCollectSound;
    [SerializeField] public AudioSource PauseSound;
    [SerializeField] public AudioSource UnPauseSound;
    [SerializeField] public AudioSource Gameover;
    [SerializeField] public AudioSource LaserDown;
    [SerializeField] public AudioSource Respawn;






    private float defaultYPos = 0;
    private float timer;
    public LogicScript logic;
    //sliding parameters
    private Vector3 hitPointNormal;

    private bool IsSliding
    {
        get
        {
            //this to check how long the raycast is onto the floor
            Debug.DrawRay(transform.position, Vector3.down , Color.red);

            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 3f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    //isCrouching
    private bool isCrouching;
    //is in crouching animation
    private bool duringCrouchAnimation;

    private Camera PlayerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = PlayerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        RunningSound.enabled = false;
        walkingSound.enabled = false;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
        if (CanMove && playerIsAlive)
        {
            if (JumpBoots == true)
            {
                HandleBootMovement();
                HandleMouselock();
            }

            else
            {
                HandleMovement();
                HandleMouselock();

            }
            
            if (canJump)
            {
                if (JumpBoots == true)
                {
                    HandleJumpBoots();
                }
                else
                {
                    HandleJump();

                }
            }

            if (canCrouch)
                HandleCrouch();

            if (canUseHeadBob)
                HandleHeadBob();

            ApplyFinalMovement();
        }
    }

    private void HandleMovement()
    {
        //the .normalized makes it diagnol movement not being faster
        if (characterController.isGrounded)
        {
            currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxisRaw("Vertical"), (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxisRaw("Horizontal")).normalized;

            float moveDirectionY = moveDirection.y;
            if (characterController.isGrounded)
                moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
            moveDirection.y = moveDirectionY;

        }
    }
    private void HandleBootMovement()
    {
        //the .normalized makes it diagnol movement not being faster
        if (characterController.isGrounded)
        {
            currentInput = new Vector2((isCrouching ? crouchBootSpeed : isSprinting ? sprintBootSpeed : walkBootSpeed) * Input.GetAxisRaw("Vertical"), (isCrouching ? crouchBootSpeed : isSprinting ? sprintBootSpeed : walkBootSpeed) * Input.GetAxisRaw("Horizontal")).normalized;

            float moveDirectionY = moveDirection.y;
            moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
            moveDirection.y = moveDirectionY;

        }
    }

    private void HandleMouselock()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * lookSpeedy;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX,0,0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("Mouse X") * lookSpeedx , 0);

    }

    private void HandleJump()
    {

        if (characterController.isGrounded)
        {
            CoyoteTimeCounter = CoyoteTime;

        }
        else
        {
            CoyoteTimeCounter -= Time.deltaTime;
        }

        if ((shouldJump))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (CoyoteTimeCounter > 0.0f && jumpBufferCounter > 0.0f)
        {

            moveDirection.y = jumpForce;

            jumpBufferCounter = 0.0f;

        }
        else if (jetPack == true)
        {
            moveDirection.y = jumpForce;
            jetPack = false;
        }

        if ((shouldJump) && moveDirection.y > 0.0f)
        {

            CoyoteTimeCounter = 0.0f;
        }

    }

    private void HandleJumpBoots()
    {
        if (jetPack == false)
        {
            if (characterController.isGrounded)
            {
                CoyoteTimeCounter = CoyoteTime;

            }
            else
            {
                CoyoteTimeCounter -= Time.deltaTime;
            }

            if ((shouldJump))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            if (CoyoteTimeCounter > 0.0f && jumpBufferCounter > 0.0f)
            {
                moveDirection.y = BootForce;
                JumpingSound.Play();
                jumpBufferCounter = 0.0f;
            }
            if ((shouldJump) && moveDirection.y > 0.0f)
            {

                CoyoteTimeCounter = 0.0f;
            }
        }
        else 
        {
            if (jetPack == true)
            {
                if ((shouldJump && !characterController.isGrounded))
                {
                    moveDirection.y = BootForce;
                    jetPack = false;
                    JetPackSound.Play();
                }
            }
        }
    }
    private void HandleCrouch()
    {
        if (shouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadBob()
    {
        if (!characterController.isGrounded) return;

        //Mathf.Abs = only positive value
        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            PlayerCamera.transform.localPosition = new Vector3(
                PlayerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                PlayerCamera.transform.localPosition.z);
        }
    }
    private void ApplyFinalMovement()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        
        if(characterController.velocity.y < -1 && characterController.isGrounded)
            moveDirection.y = -0.5f;

        if (WillSlideOnSlopes && IsSliding)
            moveDirection += new Vector3(hitPointNormal.x , -hitPointNormal.y , hitPointNormal.z) * slopeSpeed;

        if (JumpBoots == true)
        {
            characterController.Move((isCrouching ? crouchBootSpeed : isSprinting ? sprintBootSpeed : walkBootSpeed) * moveDirection * Time.deltaTime);
        }
        else
        {
            characterController.Move((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * moveDirection * Time.deltaTime);

        }

        if (characterController.isGrounded && playerIsAlive == true)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                if (isSprinting)
                {
                    RunningSound.enabled = true;
                    walkingSound.enabled = false;
                }

                else
                {
                    RunningSound.enabled = false;
                    walkingSound.enabled = true;
                }
            }
            else
            {
                RunningSound.enabled = false;
                walkingSound.enabled = false;
            }
        }
        else
        {
            RunningSound.enabled = false;
            walkingSound.enabled = false;
        }
        
    }

    private IEnumerator CrouchStand()
    {
        if(isCrouching && Physics.Raycast(PlayerCamera.transform.position, Vector3.up , 1f))
            yield break;

        duringCrouchAnimation= true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed/timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed/timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    public void PauseGame()
    {
        if (gameIsPaused)
        {
            PauseSound.Play();
            pauseLabel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CanMove = false;
        }
        else if (!gameIsPaused)
        {
            UnPauseSound.Play();
            pauseLabel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CanMove = true;

        }
    }

    public void ResumeGame()
    {
        gameIsPaused = !gameIsPaused;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CanMove = true;
    }

    public void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Boot")
        {
            JumpBoots = true;
            BootCollectSound.Play();
            Destroy(collisionInfo.gameObject);
        }

        if (collisionInfo.gameObject.tag == "PutBack")
        {
            JumpBoots = true;
            Respawn.Play();
            Destroy(collisionInfo.gameObject);
        }

        if (collisionInfo.gameObject.tag == "NoBoots")
        {
            JumpBoots = false;
        }

        if (collisionInfo.gameObject.tag == "JetPack")
        {
            jetPack = true;
            JetPackCollectSound.Play();
            Destroy(collisionInfo.gameObject);
        }
        if (collisionInfo.gameObject.tag == "Cash")
        {
            Debug.Log("you picked the cash");
            CashCollectSound.Play();
            CashCollect.charge++;
            Destroy(collisionInfo.gameObject);
        }
        if (collisionInfo.gameObject.tag == "Laser")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            LaserDown.Play();
            logic.gameOver();
            playerIsAlive = false;
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.tag == "Black")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Gameover.Play();
            logic.gameOver();
            playerIsAlive = false;
        }
    }

    public void plusKey()
    {
        KeyCollect.key++;
        KeyCollectSound.Play();
        Destroy(key);
    }

    public void Stop()
    {
        CanMove = false;
        walkingSound.enabled = false;
        RunningSound.enabled = false;
    }

    public void Continue()
    {
        CanMove = true;
    }
}
