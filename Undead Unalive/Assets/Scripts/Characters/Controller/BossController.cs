using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{

    public NavMeshAgent agent; // navmesh 
    public int detectRadius; // zone for fast running

    private GameObject player; // player reference
    private Rigidbody rb; // boss's rigidbody
    private float distance; // distance between boss and player
    private Animator animator; // boss's animator

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        detectRadius = 20;
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

    }

    // on touch with player, will kill player
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
                agent.SetDestination(this.transform.position);
                rb.velocity = new Vector3(0, 0, 0);
                Kill();
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
        // instantiate blood spill
        // destroy player game object
    }
}
