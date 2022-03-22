using Interface;
using Managers;
using UnityEngine;
using Utilities;

namespace Characters.Entity
{
    public class Player : MonoBehaviourSingleton<Player>
    {

        private CharacterEntity _playerEntity;

        private void Start()
        {
            _playerEntity = GetComponent<CharacterEntity>();
            Debug.Assert(_playerEntity != null);
            
            _playerEntity.onHealthChanged.AddListener(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged()
        {
            if (_playerEntity.GetHealth() <= 0)
            {
                GameManager.Instance.SetGameOver();
            }
        }
    }
}
