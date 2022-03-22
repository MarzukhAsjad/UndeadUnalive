using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface.Controller
{
    public class GameOverScreenController : MonoBehaviour
    {
        private CanvasGroup _uiGroup;

        private void Start()
        {
            _uiGroup = GetComponent<CanvasGroup>();
            StartCoroutine(nameof(FadeIn), 1);
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
    }
}
