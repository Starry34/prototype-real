using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public GameObject text1;

    // Start is called before the first frame update
    void Start()
    {

        text1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider touch)
    {
        if (touch.gameObject.tag == "Player")
        {

            text1.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text1.SetActive(false);
            Destroy(gameObject);
        }
    }
}
