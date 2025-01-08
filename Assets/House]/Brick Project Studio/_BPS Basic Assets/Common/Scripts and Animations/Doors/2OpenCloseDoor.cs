using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class TwoOpenCloseDoor : MonoBehaviour
    {
        public GameObject textOpenDoor;
        public Animator openandclose;
        public bool open;
        public Transform Player;

        void Start()
        {
            textOpenDoor.SetActive(false);
            open = false;
        }

        void OnMouseOver()
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

                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                StartCoroutine(opening2());
                                textOpenDoor.SetActive(false);


                            }
                        }
                        else
                        {
                            if (open == true)
                            {

                                if (Input.GetKeyDown(KeyCode.E))
                                {
                                    StartCoroutine(closing2());

                                }
                            }

                        }

                    }
                }

            }

        }

        IEnumerator opening2()
        {
            print("you are opening the door");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
        }
        IEnumerator closing2()
        {
            print("you are closing the door");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(.5f);
        }

        public void OnMouseExit()
        {
            textOpenDoor.SetActive(false);

        }
    }
}