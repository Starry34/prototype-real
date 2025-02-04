using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]


public class UImanager : MonoBehaviour
{
    private string levelToLoad;
    public AudioClip beep;
    public AudioClip unPause;
    public FirstPersonController resume;
    public GameObject pauseLabel;

    // Start is called before the first frame update
    void Start()
    {
        pauseLabel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void playBtnSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = beep;
        audio.Play();
    }

    void playUnPauseSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = unPause;
        audio.Play();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }

    public void PlayBtnClicked()
    {
        levelToLoad = "House";
        playBtnSound();
        StartCoroutine(Wait());
        SceneManager.LoadScene(levelToLoad);
    }

    public void InsBtnClicked()
    {
        levelToLoad = "Instruction";
        playBtnSound();
        StartCoroutine(Wait());
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitBtnClicked()
    {
        levelToLoad = "";
        playBtnSound();
        StartCoroutine(Wait());
        Application.Quit();
        Debug.Log("This park works!");
    }
    public void InsBackBtnClicked()
    {
        levelToLoad = "MainMenu";
        playBtnSound();
        StartCoroutine(Wait());
        SceneManager.LoadScene(levelToLoad);
    }
    public void ResumeBtnClicked()
    {
        resume.ResumeGame();
        pauseLabel.SetActive(false);
        playUnPauseSound();
        StartCoroutine(Wait());
    }
}

