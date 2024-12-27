using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 5.0f;
    [SerializeField] private float speed = 5f;
    //adjust this to change how high it goes
    [SerializeField] private float height = 0.5f;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f,rotationAmount,0.0f);

        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z); 
    }
}
