using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMouse : MonoBehaviour
{

    public GameObject textOpenDoor;
    public GameObject textCloseDoor;
    public bool open;
    public Transform Player;


    // Start is called before the first frame update
    void Start()
    {
        textOpenDoor.SetActive(false);
        textCloseDoor.SetActive(false);
        open = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        {
            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 3)
                {
                    if (open == false)
                    {
                        textOpenDoor.SetActive(true);
                    }
                    else
                    {
                        if (open == true)
                        {
                            textCloseDoor.SetActive(true);
                        }

                    }

                }
            }

        }
    }

    public void OnMouseExit()
    {
        textOpenDoor.SetActive(false);
        textCloseDoor.SetActive(true);

    }
}
