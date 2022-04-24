using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PickUpGrenade : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddDeltaScore(20, "pickup");
            ScoringSystem.grenadeCount += 1;
            gameObject.SetActive(false);
        }
    }
}
