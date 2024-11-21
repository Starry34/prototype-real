using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.001f;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        offset = transform.position - target.position ;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //calculate the desired position with the offset
        Vector3 desiredPosition = target.position + offset;

        //smoothly transition to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        //Rotate to look at the target
        transform.LookAt(target);

    }
}
