using System;
using Characters.Entity;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Interface
{
    public class PlayerHUDController : MonoBehaviourSingleton<PlayerHUDController>
    {
        private GameObject _playerHUDObject;
        private CharacterEntity _playerEntity;

        private GameObject _healthBar;
        private Slider _healthBarSlider;
        private RectTransform _healthBarRect;
        private float _trackingMaxHealth;
        private float _defaultHealthBarWidth;
        private float _defaultHealthBarPosX;

        // Start is called before the first frame update
        void Start()
        {
            _playerHUDObject = GameObject.FindWithTag("PlayerHUD")?.gameObject;
            Debug.Assert(_playerHUDObject != null);

            _playerEntity = GameObject.FindWithTag("Player")?.GetComponent<CharacterEntity>();
            Debug.Assert(_playerEntity != null);
            _playerEntity.onHealthChanged.AddListener(OnHealthChange);

            _healthBar = _playerHUDObject.transform.Find("HealthBar").gameObject;
            _healthBarSlider = _healthBar.GetComponent<Slider>();
            _healthBarRect = _healthBar.GetComponent<Slider>().GetComponent<RectTransform>();
            var rect = _healthBarRect.rect;
            _defaultHealthBarWidth = _healthBarRect.sizeDelta.x;
            _defaultHealthBarPosX = _healthBarRect.anchoredPosition.x;
            _trackingMaxHealth = _playerEntity.GetDefaultMaxHealth();

            OnHealthChange();
        }

        private void OnHealthChange()
        {
            var maxHealth = _playerEntity.GetMaxHealth();

            if (Math.Abs(_trackingMaxHealth - maxHealth) > float.Epsilon)
            {
                _trackingMaxHealth = maxHealth;
                
                var newWidth = _defaultHealthBarWidth * (maxHealth / _playerEntity.GetDefaultMaxHealth());

                var temAnchor = _healthBarRect.anchoredPosition;
                temAnchor.x = _defaultHealthBarPosX + (newWidth - _defaultHealthBarWidth) / 2;
                _healthBarRect.anchoredPosition = temAnchor;
                
                
                var temSizeDelta = _healthBarRect.sizeDelta;
                temSizeDelta.x = newWidth;
                _healthBarRect.sizeDelta = temSizeDelta;
            }

            _healthBarSlider.value = _playerEntity.GetHealth() / _playerEntity.GetMaxHealth();
        }

        // Update is called once per frame
        void Update() { }
    }
}
