using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    [SerializeField] Transform platform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collisioninfo)
    {
        if (collisioninfo.gameObject.tag == "Player")
        {
            collisioninfo.gameObject.transform.parent = platform;
        }
        
    }

    private void OnTriggerExit(Collider collisioninfo)
    {
        if (collisioninfo.gameObject.tag == "Player")
        {
            collisioninfo.gameObject.transform.parent = null;
        }
    }
}
