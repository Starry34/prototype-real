using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueAfterPaper : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public FirstPersonController move;
    public TextPopUp DialogueTrue;
    public GameObject DialogueBox;
    public string[] lines;
    public float textspeed;
    public AudioSource RPGTEXT;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        DialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (textComponent.text == lines[index])
            {
                BeforeLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        DialogueBox.SetActive(true);
        move.Stop();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            RPGTEXT.Play();
            yield return new WaitForSeconds(textspeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            DialogueBox.SetActive(false);
            DialogueTrue.DialogueFalse();
            move.Continue();
        }
    }
    void BeforeLine()
    {
        if (index < lines.Length + 1)
        {
            if (index > 0)
            {

                index--;
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            }
        }
        else
        {
            DialogueBox.SetActive(false);
        }
    }
}
