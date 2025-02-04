using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisText : MonoBehaviour
{

    public GameObject text1;
    public GameObject blurry;

    // Start is called before the first frame update
    void Start()
    {

        text1.SetActive(false);
        blurry.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision touch)
    {
        text1.SetActive(true);
        blurry.SetActive(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        text1.SetActive(false);
        blurry.SetActive(false);

    }
}
