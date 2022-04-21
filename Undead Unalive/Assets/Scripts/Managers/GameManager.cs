using System;
using Interface;
using UnityEngine;
using UserInterface;
using Utilities;

namespace Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public int TargetFPS { private set; get; } = 160;
        private bool _isGameOver = false;

        [SerializeField] private GameObject pauseScreen;

        private void Start()
        {
            Application.targetFrameRate = TargetFPS;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen.SetActive(true);
            }
        }

        public void SetGameOver()
        {
            if (!_isGameOver)
            {
                InputManager.Instance.DisableAllUserInput();
                PlayerHUDController.Instance.DoGameOverScreen();
            }

            _isGameOver = true;
        }
    }
}
