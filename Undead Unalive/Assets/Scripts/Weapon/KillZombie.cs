using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class KillZombie : MonoBehaviour
{

    public GameObject BanditPrefab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vaccine"))
        {
            ScoreManager.Instance.AddDeltaScore(10, "kill zombie");
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(BanditPrefab, transform.position, transform.rotation);
        }
    }
}
