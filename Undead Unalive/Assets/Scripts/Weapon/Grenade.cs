using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float delay = 2f;
    public float radius = 5f;

    float countdown;

    bool hasExploded = false;

    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <=0f && !hasExploded)
        {
            Debug.Log("Boom");
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        // Show explosion effect

        Collider[] colliders =  Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Zombie")
            {
                Destroy(nearbyObject.gameObject);
            }
        }
        // Get nearby object and damage them

        // Remove grenade

        Destroy(gameObject);
    }
}
