using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaughterKey : MonoBehaviour
{
    [SerializeField] private GameObject Emage;
    [SerializeField] private GameObject BlurImage;
    [SerializeField] public Transform Player;
    [SerializeField] public FirstPersonController keyCollect;


    // Start is called before the first frame update
    void Start()
    {
        Emage.SetActive(false);
        BlurImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void OnMouseOver()
    {
        if (Player)
        {
            float dist = Vector3.Distance(Player.position, transform.position);
            if(dist < 4)
            {
                Emage.SetActive(true);
                BlurImage.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    keyCollect.plusKey(); 
                }
            }
        }
    }

    public void OnMouseExit()
    {
        Emage.SetActive(false);
        BlurImage.SetActive(false);

    }
}
