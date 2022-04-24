using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PickupMask : MonoBehaviour
{
    // Start is called before the first frame update
    Characters.Entity.CharacterEntity player;

    private void OnTriggerEnter(Collider other)
    {
        ScoreManager.Instance.AddDeltaScore(20);
        ScoringSystem.maskCount += 1;
        gameObject.SetActive(false);
    }
}
