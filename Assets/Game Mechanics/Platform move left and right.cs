using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformmoveleftandright : MonoBehaviour
{
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
        float newX = Mathf.Sin(Time.time * speed) * height + pos.x;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
