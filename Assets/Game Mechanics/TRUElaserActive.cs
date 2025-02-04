using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRUElaserActive : MonoBehaviour
{
    public GameObject laserActive;

    // Start is called before the first frame update
    void Start()
    {
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
        }

    }
}
