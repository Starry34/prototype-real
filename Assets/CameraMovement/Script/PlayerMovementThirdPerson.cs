using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementThirdPerson : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotateSpeed = 0.5f;
    public float jumpForce = 150.0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotatex = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        transform.position += transform.forward * Time.deltaTime * speed * moveZ;
        transform.Rotate(rotatex * rotateSpeed * Vector3.up);
    }
}
