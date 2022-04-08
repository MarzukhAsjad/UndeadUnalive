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

        private void Start()
        {
            Application.targetFrameRate = TargetFPS;
        }

        public void SetGameOver()
        {
            if (!_isGameOver)
            {
                InputManager.Instance.DisabledAllUserInput();
                PlayerHUDController.Instance.DoGameOverScreen();
            }

            _isGameOver = true;
        }
    }
}
