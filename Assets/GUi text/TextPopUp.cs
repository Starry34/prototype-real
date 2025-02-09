
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    public bool open;
    public Transform Player;
    public GameObject Image;
    public GameObject Exclamation;
    public DialogueAfterPaper DialogueTrue;
    public FirstPersonController StopMoving;
    public bool DialogueOpen;
    public static int counter;
    [SerializeField] private AudioSource paper;
    [SerializeField] private AudioSource paperDown;


    // Start is called before the first frame update
    void Start()
    {
        Exclamation.SetActive(true);
        Image.SetActive(false);
        open = false;
        DialogueOpen = true;
        counter = 0;

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
                    if (open == false)
                    {
                        if (DialogueOpen == true)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                StartCoroutine(ImageOnScreen());
                            }
                        }
                    }
                    else if (open == true)
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
    }

    IEnumerator ImageOnScreen()
    {
        Image.SetActive(true);
        Exclamation.SetActive(false);
        StopMoving.Stop();
        paper.Play();
        open = true;
        DialogueOpen = false;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator NoImageOnScreen()
    {
        Image.SetActive(false);
        StopMoving.Continue();
        open = false;
        paperDown.Play();
        DialogueTrue.StartDialogue();
        
        yield return new WaitForSeconds(.5f);
    }

    public void DialogueFalse()
    {
        DialogueOpen = true;
    }
}
