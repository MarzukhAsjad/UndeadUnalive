using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
    {
        public float initialScore = 100;
        private float _score;
        private int numSurvivors = 1;
        private int initialSurvivors = 1;

        [SerializeField] private float scoreDefaultSize;
        [SerializeField] private TextMeshProUGUI scoreObject;

        [SerializeField] private float multiplierDefaultSize;
        [SerializeField] private TextMeshProUGUI multiplierObject;

        private Coroutine _scoreAnimation;
        private Coroutine _multiplierAnimation;

        private void Start()
        {
            _score = initialScore;
            AddDeltaScore(0);
        }

        private void Update()
        {
            
            if (initialSurvivors != numSurvivors)
            {
                initialSurvivors = numSurvivors;
                ChangeMultiplier(numSurvivors);
            }
        }

        public void AddDeltaScore(float delta, string reason = "")
        {
            if (reason.Length != 0)
            {
                Debug.Log("Score: " + _score + " -> " + Mathf.Min(_score + delta, 99999) + " because " + reason + ".");
            }

            _score += delta;
            _score = Mathf.Min(_score, 99999);

            ChangeScore(_score);
        }

        public void RegisterNewMob()
        {
            numSurvivors += 1;
        }

        public void KillMob()
        {
            numSurvivors -= 1;
        }

        private void ChangeScore(float to)
        {
            if (_scoreAnimation != null) StopCoroutine(_scoreAnimation);

            scoreObject.fontSize = scoreDefaultSize;
            _scoreAnimation = StartCoroutine(TextAnimation(scoreObject, to, 0.2f));
        }

        private void ChangeMultiplier(float to)
        {
            if (_multiplierAnimation != null) StopCoroutine(_multiplierAnimation);

            multiplierObject.fontSize = multiplierDefaultSize;
            _multiplierAnimation = StartCoroutine(TextAnimation(multiplierObject, to, 0.2f));
        }

        private IEnumerator TextAnimation(TextMeshProUGUI targetObject, float to, float time = 1)
        {
            float timer = 0;
            float defaultSize = targetObject.fontSize;
            float maxSize = defaultSize * 1.5f;

            float defaultValue = float.Parse(targetObject.text);
            float deltaChange = to - defaultValue;

            while (timer < time)
            {
                targetObject.fontSize = defaultSize + (maxSize - defaultSize) * Mathf.Sin(Mathf.PI * (timer / time));
                targetObject.text =
                    ((int)(defaultValue + deltaChange * (timer / time))).ToString(CultureInfo.InvariantCulture);

                yield return new WaitForFixedUpdate();
                timer += Time.deltaTime;
            }

            targetObject.text = to.ToString(CultureInfo.InvariantCulture);
            targetObject.fontSize = defaultSize;
        }
    }
}