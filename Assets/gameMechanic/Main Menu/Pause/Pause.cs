using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject pauseLabel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
      if (gameIsPaused)
        {
            pauseLabel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pauseLabel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}

