using UnityEngine;
using UnityEngine.Events;

namespace Characters.Entity
{
    public class CharacterEntity : MonoBehaviour
    {
        private const float DefaultMaxHealth = 100;
        private float _maxHealth = DefaultMaxHealth;
        
        private const float DefaultMaxStamina  = 100;
        private float _maxStamina = DefaultMaxStamina;

        public UnityEvent onHealthChanged = new();
        private float _health = DefaultMaxHealth;
        
        public UnityEvent onStaminaChanged = new();
        private float _stamina = DefaultMaxStamina;

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
        
        public void ChangeStamina(float newStamina)
        {
            _stamina = newStamina;
            onStaminaChanged?.Invoke();
        }

        public void AddDeltaStamina(float deltaStamina)
        {
            ChangeStamina(_stamina + deltaStamina);
        }

        public void ChangeMaxStamina(float newMaxStamina)
        {
            _maxStamina = newMaxStamina;
            onStaminaChanged?.Invoke();
        }

        public void AddDeltaMaxStamina(float deltaMaxStamina)
        {
            ChangeMaxStamina(_maxStamina + deltaMaxStamina);
        }

        public float GetStamina()
        {
            return _stamina;
        }

        public float GetDefaultMaxStamina()
        {
            return DefaultMaxStamina;
        }

        public float GetMaxStamina()
        {
            return _maxStamina;
        }
    }
}
