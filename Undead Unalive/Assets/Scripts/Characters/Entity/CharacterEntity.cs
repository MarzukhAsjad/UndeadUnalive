using UnityEngine;

namespace Characters.Entity
{
    public class CharacterEntity : MonoBehaviour
    {
        private float _health = 0;

        public void ChangeHealth(float newHealth)
        {
            _health = newHealth;
        }

        public void AddDeltaHealth(float deltaHealth)
        {
            _health += deltaHealth;
        }
    }
}
