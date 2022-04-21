using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZombie : MonoBehaviour
{

    public GameObject BanditPrefab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vaccine"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(BanditPrefab, transform.position, transform.rotation);
            Debug.Log("Shoot");
        }
    }
}
