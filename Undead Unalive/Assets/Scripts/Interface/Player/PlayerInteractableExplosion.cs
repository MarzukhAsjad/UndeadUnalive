using UnityEngine;

namespace Interface.Player
{
    public class PlayerInteractableExplosion : PlayerInteractable
    {
        public override void OnInteract(GameObject target, Vector3 hitPosition)
        {
            Debug.Log("Explosion OnInteract() called by target:" + target);
            gameObject.GetComponent<Rigidbody>().AddExplosionForce(10, hitPosition, 10, 5, ForceMode.VelocityChange);
        }
    }
}