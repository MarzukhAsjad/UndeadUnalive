using UnityEngine;
using UnityEngine.Events;

namespace Characters.Entity
{
    public class CharacterEntity : MonoBehaviour
    {
        private const float DefaultMaxHealth = 100;
        private float _maxHealth = DefaultMaxHealth;

        public UnityEvent onHealthChanged = new();
        private float _health = DefaultMaxHealth;

        public void ChangeHealth(float newHealth)
        {
            _health = newHealth;
            onHealthChanged?.Invoke();
        }

        public void AddDeltaHealth(float deltaHealth)
        {
            ChangeHealth(_health + deltaHealth);
        }

        public void ChangeMaxHealth(float newMaxHealth)
        {
            _maxHealth = newMaxHealth;
            onHealthChanged?.Invoke();
        }

        public void AddDeltaMaxHealth(float deltaMaxHealth)
        {
            ChangeMaxHealth(_maxHealth + deltaMaxHealth);
        }

        public float GetHealth()
        {
            return _health;
        }

        public float GetDefaultMaxHealth()
        {
            return DefaultMaxHealth;
        }

        public float GetMaxHealth()
        {
            return _maxHealth;
        }

        private void Update()
        {
            AddDeltaMaxHealth(Time.deltaTime);
            AddDeltaHealth(Time.deltaTime / 2);
        }
    }
}
