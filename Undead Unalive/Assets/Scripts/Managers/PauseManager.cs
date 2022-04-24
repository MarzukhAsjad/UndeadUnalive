using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Utilities;

public class PauseManager : MonoBehaviourSingleton<PauseManager>
{
    [SerializeField] private GameObject pauseScreenOptionMenu;

    private void OnEnable()
    {
        Time.timeScale = 0;
        InputManager.ReleaseMouse();

        InputManager.Instance.DisableAllUserInput();
    }

    public void ActivateOptionMenu()
    {
        pauseScreenOptionMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        InputManager.LockMouse();

        gameObject.SetActive(false);
        InputManager.Instance.EnableAllUserInput();
        GameManager.Instance.isPaused = false;
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