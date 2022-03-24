using Characters.Entity;
using UnityEngine;

namespace Interface.Player
{
    public class PlayerInteractable : MonoBehaviour
    {
        public virtual void OnInteract(GameObject target)
        {
            Debug.Log("default OnInteract() called by target:" + target);
        }
    }
}
