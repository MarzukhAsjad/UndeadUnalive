using Characters.Controller;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    public GameObject[] escapingMobs;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        escapingMobs = GameObject.FindGameObjectsWithTag("Mob");
        foreach (GameObject mob in escapingMobs)
        {
            ScoreManager.Instance.AddDeltaScore(20, "Escaped");
            Debug.Log("Escape");
            Destroy(mob);
        }
    }
}
