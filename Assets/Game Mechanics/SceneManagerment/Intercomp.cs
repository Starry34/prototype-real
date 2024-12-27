using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SojaExiles
{
    public class Intercomp : MonoBehaviour
    {
        private string levelToLoad;
        public Transform Player;
        public static bool Cash = false;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (CashCollect.charge >= 1)
            {
                Cash = true;
            }
        }
        void OnMouseOver()
        {
            {
                if (Player)
                {
                    float dist = Vector3.Distance(Player.position, transform.position);
                    if (dist < 5 && Cash == true)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            levelToLoad = "House";
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
