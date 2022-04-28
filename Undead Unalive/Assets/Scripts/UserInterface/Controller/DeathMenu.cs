using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{  
    void Start()
    {
        // should pause the game
        Time.timeScale = 0.0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    } 

    public void EndGame()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
