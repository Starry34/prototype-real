using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement
    public CharacterController charController;
    public float charSpeed = 6.0f;
    public float charGravity = -9.8f;
    private Vector3 velocity;

    //jump

    public float jumpHeight = 1.0f;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Debug.Log(moveX);
            Debug.Log(moveZ);

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        charController.Move(move * charSpeed * Time.deltaTime);

        velocity.y += charGravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);

        isGrounded = charController.isGrounded;
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * charGravity);
        }

        velocity.y += charGravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }
}
