using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGrenade : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoringSystem.grenadeCount += 1;
            gameObject.SetActive(false);
        }
    }
}
