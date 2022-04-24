using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public NavMeshAgent agent; // navmesh 
    public int detectRadius; // zone for fast running
    public int health; // boss health
    public GameObject bossHealth;

    private GameObject player; // player reference
    private Rigidbody rb; // boss's rigidbody
    private float distance; // distance between boss and player
    private Animator animator; // boss's animator
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth.SetActive(true);
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        slider = bossHealth.GetComponent<Slider>();
        detectRadius = 20;
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance > detectRadius) 
        {

            agent.SetDestination(player.transform.position);
            Walk();

        }
        else 
        {
            agent.SetDestination(player.transform.position);
            Run();
        }

        if (health < 1)
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player") // to detect player
        {
                agent.SetDestination(this.transform.position);
                rb.velocity = new Vector3(0, 0, 0);
                Kill();
        }

        if (collider.tag == "Vaccine") // to detect vaccine
        {
            Destroy(collider.gameObject);
            health -= 1;
            changeHealth();

        }

        
    }

    //make the boss walk
    public void Walk() // set speed to 2
    {
        agent.speed = 2.0f;
        animator.SetBool("walk", true);
        animator.SetBool("run", false); 
    }

    //make the boss run
    public void Run() // set speed to 3.5
    {
        agent.speed = 3.5f;
        animator.SetBool("walk", false);
        animator.SetBool("run", true); 
    }

    //make the boss do nothing, stand idle and EAT
    public void Idle() // set speed to 0
    {
        agent.speed = 0f;
        animator.SetBool("walk", false);
        animator.SetBool("run", false); 
    }

    public void Kill()
    {
        agent.speed = 0f;
        animator.SetBool("eat", true);
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        // instantiate blood spill\

        // destroy player game object
    }

    public void Death()
    {
        agent.speed = 0f;
        animator.SetBool("death", true);
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("eat", false);
        StartCoroutine(DelayDeactivate());
    }
    public void changeHealth()
    {
        slider.value = .1f * (float)health;
    }

    IEnumerator DelayDeactivate()
    {
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
        bossHealth.SetActive(false);
    }
}
