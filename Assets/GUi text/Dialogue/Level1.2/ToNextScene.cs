using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ToNextScene : MonoBehaviour
{
    private string levelToLoad;
    public Transform Player;
    public GameObject exclamation;
    [SerializeField] private string level;



    // Start is called before the first frame update
    void Start()
    {
        exclamation.SetActive(true);
    }

    // Update is called once per frame
    void Update()
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
                        levelToLoad = level;
                        SceneManager.LoadScene(levelToLoad);
                    }
                }

            }

        }
    }
}


