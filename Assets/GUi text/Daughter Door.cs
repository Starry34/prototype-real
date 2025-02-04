using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class DaughterDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public GameObject imgLockedDoor;
        public GameObject imgUnlockedDoor;
        public AudioSource KeyUnlock;
        public AudioSource KeyLock;
        public static bool Daughter;


        public static bool key = false;
        [SerializeField] private int keyValue;




        void Start()
        {
            open = false;
            Daughter = false;
            imgUnlockedDoor.SetActive(false);
            imgLockedDoor.SetActive(false);

        }

        void Update()
        {
            if (KeyCollect.key >= keyValue)
            {
                Daughter = true;
            }
        }

        void OnMouseOver()
        {
            {
                if (Player)
                {
                    float dist = Vector3.Distance(Player.position, transform.position);
                    if (dist < 3)
                    {
                        if (open == false && Daughter == true)
                        {
                            imgUnlockedDoor.SetActive(true);

                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                StartCoroutine(opening());
                                imgUnlockedDoor.SetActive(false);


                            }
                        }
                        else
                        {
                            if (open == false && Daughter == false)
                            {
                                imgLockedDoor.SetActive(true);
                                if (Input.GetKeyDown(KeyCode.E))
                                {
                                    StartCoroutine(locking());
                                }
                            }

                        }

                    }
                }

            }

        }

        IEnumerator opening()
        {
            print("you are opening the door");
            openandclose.Play("Opening");
            KeyUnlock.Play();
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator locking()
        {
            print("the door is locked");
            KeyLock.Play();
            yield return new WaitForSeconds(.5f);
        }
        public void OnMouseExit()
        {
            imgLockedDoor.SetActive(false);
            imgUnlockedDoor.SetActive(false);

        }
    }
}