using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogueEnding : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject DialogueBox;
    public string[] lines;
    public float textspeed;
    public AudioSource rpgText;
    private string levelToLoad;
    [SerializeField] private string level;



    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        DialogueBox.SetActive(false);
        StartDialogue();
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
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            rpgText.Play();
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
            levelToLoad = level;
            SceneManager.LoadScene(levelToLoad);

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
