using Characters.Entity;
using Interface.Controller;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Interface
{
    public class PlayerHUDController : MonoBehaviourSingleton<PlayerHUDController>
    {
        /*
         *
         *  Prefab
         * 
         */
        [SerializeField] private GameObject gameOverScreen;

        private GameObject _playerHUDObject;
        private GameObject _healthBar;

        private CharacterEntity _playerEntity;

        private readonly UnityEvent<float, float> _healthChangeProxy = new();

        private void OnEnable()
        {
            _playerHUDObject = GameObject.FindWithTag("PlayerHUD")?.gameObject;
            Debug.Assert(_playerHUDObject != null);

            _playerEntity = GameObject.FindWithTag("Player")?.GetComponent<CharacterEntity>();
            Debug.Assert(_playerEntity != null);
            
            _healthBar = _playerHUDObject.transform.Find("HealthBar").gameObject;
            
            _playerEntity.onHealthChanged.AddListener(() => _healthChangeProxy.Invoke(_playerEntity.GetHealth(), _playerEntity.GetDefaultMaxHealth()));
            var barController = _healthBar.AddComponent<BarController>();
            barController.changeEvent = _healthChangeProxy;
            barController.defaultValue = _playerEntity.GetHealth();
            barController.defaultMax = _playerEntity.GetDefaultMaxHealth();
        }

        public void DoGameOverScreen()
        {
            Instantiate(gameOverScreen);
        }
    }
}
