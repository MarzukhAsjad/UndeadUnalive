using Interface;
using Managers;
using UnityEngine;
using Utilities;

namespace Characters.Entity
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        private CharacterEntity _playerEntity;
        public float targetTime = 3f;
        private int threshold = 0;


        private void Start()
        {
            _playerEntity = GetComponent<CharacterEntity>();
            Debug.Assert(_playerEntity != null);

            _playerEntity.onHealthChanged.AddListener(OnPlayerHealthChanged);
        }

        private void Update()
        {
        }

        private void OnPlayerHealthChanged()
        {
            if (_playerEntity.GetHealth() <= 0)
            {
                GameManager.Instance.SetGameOver();
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("ToxicGas"))
            {
                Debug.Log("Lol");
                PlayerDamage();
            }
        }

        private void PlayerDamage()
        {
            if (ScoringSystem.maskCount >= 1)
            {
                threshold += 1;
                if (threshold == 15)
                {
                    ScoringSystem.maskCount -= 1;
                    threshold = 0;
                }
            }
            else
            {
                _playerEntity.AddDeltaHealth(-1.0f);
            }
        }
    }
}