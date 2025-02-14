﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseDoor : MonoBehaviour
	{
        public Animator openandclose;
		public bool open;
		public Transform Player;
        public GameObject imgOpenDoor;
		public AudioSource DoorOpen;
		public AudioSource DoorClose;


        void Start()
		{
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
                            imgOpenDoor.SetActive(true);

                            if (Input.GetKeyDown(KeyCode.E))
							{
								StartCoroutine(opening());
                                imgOpenDoor.SetActive(false);


                            }
                        }
						else
						{
							if (open == true)
							{
                                if (Input.GetKeyDown(KeyCode.E))
                                {
									StartCoroutine(closing());
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
			DoorOpen.Play();
			open = true;
            yield return new WaitForSeconds(.5f);
		}
		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose.Play("Closing");
			DoorClose.Play();
			open = false;
            yield return new WaitForSeconds(.5f);
		}

        public void OnMouseExit()
        {
            imgOpenDoor.SetActive(false);

        }
    }
}