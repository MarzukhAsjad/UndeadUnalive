using UnityEngine;

namespace Utilities
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : class
    {
        private static MonoBehaviourSingleton<T> _instance;
        public static T Instance => _instance as T;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
 
            if (_instance != null && !ReferenceEquals(_instance, this))
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}
