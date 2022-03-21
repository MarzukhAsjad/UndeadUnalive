using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public interface ICustomEvent
    {
        [System.Serializable]
        public class FloatEvent : UnityEvent<float> { }
    }
}
