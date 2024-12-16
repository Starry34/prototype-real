using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove {  get; private set; } = true;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey) & characterController.isGrounded;
    private bool shouldJump => Input.GetKey(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

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
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float gravity = 25.0f;

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

    private float defaultYPos = 0;
    private float timer;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HandleMovement();
            HandleMouselock();
            
            if (canJump)
                HandleJump();

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
        if (shouldJump)
            moveDirection.y = jumpForce;
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
        if(!characterController.isGrounded)
            moveDirection.y -= gravity *Time.deltaTime;
        
        if(characterController.velocity.y < -1 && characterController.isGrounded)
            moveDirection.y = 0;

        if(WillSlideOnSlopes && IsSliding)
            moveDirection += new Vector3(hitPointNormal.x , -hitPointNormal.y , hitPointNormal.z) * slopeSpeed;

        characterController.Move((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * moveDirection * Time.deltaTime);
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
}
