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
								levelToLoad = "Level 1";
								StartCoroutine(teleportToLab());
								SceneManager.LoadScene(levelToLoad);
							}
					}

				}

			}

			IEnumerator teleportToLab()
			{
				yield return new WaitForSeconds(3.0f);
			}

		}
	}
}