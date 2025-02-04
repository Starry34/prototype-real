using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SojaExiles

{
	public class opencloseDoor1 : MonoBehaviour
	{

		public Animator openandclose1;
		public bool open;
		public Transform Player;

		private string levelToLoad;
		[SerializeField]private string where;
		public AudioSource bigDoorOpen;

		void Start()
		{
			
		}

		void OnMouseOver()
		{
			{
				if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 3)
					{
							if (Input.GetKeyDown(KeyCode.E))
							{
								bigDoorOpen.Play();
								StartCoroutine(teleportToLab());
							}
					}

				}

			}

			IEnumerator teleportToLab()
			{
                levelToLoad = where;
                SceneManager.LoadScene(levelToLoad);
                yield return new WaitForSeconds(5.0f);
            }

        }
	}
}