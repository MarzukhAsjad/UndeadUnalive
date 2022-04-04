using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMask : MonoBehaviour
{
    // Start is called before the first frame update
    Characters.Entity.CharacterEntity player;

    private void OnTriggerEnter(Collider other)
    {
        player.ChangeHealth(100);
        Destroy(gameObject);
    }
}
