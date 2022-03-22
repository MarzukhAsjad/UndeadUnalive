using Interface;
using Utilities;

namespace Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {

        private bool _isGameOver = false;

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
