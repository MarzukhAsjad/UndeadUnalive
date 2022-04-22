using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PauseManager : MonoBehaviourSingleton<PauseManager>
{
    private void Start()
    {
        Time.timeScale = 0;
        InputManager.ReleaseMouse();

        InputManager.Instance.DisableAllUserInput();
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        InputManager.LockMouse();

        gameObject.SetActive(false);
        InputManager.Instance.EnableAllUserInput();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
