using UnityEngine;
using Utilities;

namespace Managers
{
    public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
    {
        public float initialScore = 100;
        private float _score;

        private void Start()
        {
            _score = initialScore;
        }

        public void AddDeltaScore(float delta, string reason = "")
        {
            if (reason.Length != 0)
            {
                Debug.Log("Health: " + _score + " -> " + (_score + delta) + " because " + reason + ".");
            }
        
            _score += delta;
        }
    }
}