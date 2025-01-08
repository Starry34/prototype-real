using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics.Contracts;

public class LogicScript : MonoBehaviour
{
    public static LogicScript instance;
    public Vector3 LastCheckPointPos;
    public GameObject GameOverScreen;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void gameOver()
    {
        GameOverScreen.SetActive(true);
    }
}
