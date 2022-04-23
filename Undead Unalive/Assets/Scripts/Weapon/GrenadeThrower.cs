using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 20f;

    public GameObject grenade;

    // Update is called once per frame
    void Update()
    {
        // disable throwing when game paused
        if (GameManager.Instance.isPaused) return;
        
        if (Input.GetKey(KeyCode.G) && ScoringSystem.grenadeCount>=1)
        {
            Debug.Log("BOOM");
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        GameObject grenadeObject = Instantiate(grenade, transform.position, transform.rotation);
        Rigidbody rb = grenadeObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        ScoringSystem.grenadeCount -= 1;
    }
}
