using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercomp : MonoBehaviour
{
    public Transform Player;
    public GameObject exclamation;
    public IntercompDialogue DialogueTrue;
    public bool DialogueOpen;



    // Start is called before the first frame update
    void Start()
    {
        exclamation.SetActive(true);
        DialogueOpen = true;
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
                    if (DialogueOpen == true)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            StartCoroutine(ImageOnScreen());
                        }
                    }
                    else
                    {
                        if (DialogueOpen == false)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                StartCoroutine(NoImageOnScreen());
                            }
                        }
                    }
                }
            }
        }

    }

    void OnMouseExit()
    {
        exclamation.SetActive(false);
    }

    IEnumerator ImageOnScreen()
    {
        DialogueTrue.StartDialogue();
        DialogueOpen = false;

        yield return new WaitForSeconds(.5f);
    }

    IEnumerator NoImageOnScreen()
    {
        yield return new WaitForSeconds(.5f);
    }
    public void DialogueFalse()
    {
        DialogueOpen = true;
    }
}
