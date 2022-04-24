using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float delay = 2f;
    public float radius = 5f;

    float countdown;

    bool hasExploded = false;

    public Renderer rend;

    public GameObject explosionEffect;
    public AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        explosionSound = GetComponent<AudioSource>();

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
                ScoreManager.Instance.AddDeltaScore(10);
                Destroy(nearbyObject.gameObject);
            }
        }
        // Get nearby object and damage them

        // Remove grenade

        explosionSound.Play();

        rend.enabled = false;

        Destroy(gameObject, explosionSound.clip.length);
    }
}
