using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interface.Controller
{
    public class GameOverScreenController : MonoBehaviour
    {
        private CanvasGroup _uiGroup;
        [SerializeField] private GameObject menuButton;
        [SerializeField] private GameObject quitButton;

        private void Start()
        {
            _uiGroup = GetComponent<CanvasGroup>();
            StartCoroutine(nameof(FadeIn), 1);
            ButtonFadeIn(menuButton, 0f, 0.5f);
            ButtonFadeIn(quitButton, 0.1f, 0.5f);
            InputManager.ReleaseMouse();
        }

        private IEnumerator FadeIn(float time)
        {
            for (var i = 0; i < 100; ++i)
            {
                _uiGroup.alpha = i / 100.0f;
                yield return new WaitForSeconds(time / 100);
            }

            _uiGroup.alpha = 1;
        }

        private void ButtonFadeIn(GameObject button, float delay = 0, float time = 1)
        {
            var sizeDelta = button.GetComponent<RectTransform>().anchoredPosition;
            var temHeight = sizeDelta.y;
            sizeDelta.y = -50;
            button.GetComponent<RectTransform>().anchoredPosition = sizeDelta;
            
            StartCoroutine(GoToYPosition(button, -50, temHeight, delay,
                time));
        }

        private IEnumerator GoToYPosition(GameObject button, float from, float to, float delay = 0, float time = 1)
        {
            yield return new WaitForSeconds(delay);
            var component = button.GetComponent<RectTransform>();

            for (var i = 0; i < 100; ++i)
            {
                var transformSizeDelta = component.anchoredPosition;
                transformSizeDelta.y = from + (to - from) * (i / 100.0f);
                component.anchoredPosition = transformSizeDelta;

                yield return new WaitForSeconds(time / 100);
            }

            _uiGroup.alpha = 1;
        }
        
        public void ToMenu()
        {
            SceneManager.LoadScene(0);
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
}