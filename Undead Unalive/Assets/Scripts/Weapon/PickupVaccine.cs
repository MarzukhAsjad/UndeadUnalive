using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupVaccine : MonoBehaviour
{
    // Start is called before the first frame update

    Characters.Entity.CharacterEntity player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoringSystem.vaccineCount += 1;
            gameObject.SetActive(false);
        }
    }
}
