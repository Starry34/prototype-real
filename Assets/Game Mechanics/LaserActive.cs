using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActive : MonoBehaviour
{

    public GameObject DUCK;
    public GameObject laserActive;
    // Start is called before the first frame update
    void Start()
    {
            DUCK.SetActive(false);
            laserActive.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collisioninfo)
    {
        if (collisioninfo.gameObject.tag == "Player")
        {
            laserActive.SetActive(true);
            DUCK.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider collisioninfo)
    {
        if (collisioninfo.gameObject.tag == "Player")
        {
            laserActive.SetActive(false);
            DUCK.SetActive(false);
        }
    }
}
