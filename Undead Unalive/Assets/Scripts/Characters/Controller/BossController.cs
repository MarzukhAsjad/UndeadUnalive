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
        else if (distance > 1)
        {
            agent.SetDestination(player.transform.position);
            Run();
        }
        else
        { //if distance is less than 1, stop

            agent.SetDestination(this.transform.position);
            rb.velocity = new Vector3(0, 0, 0);
            Idle();
        }

    }

    //make the boss walk
    public void Walk()
    {
        animator.SetBool("Monster_anim|Walk", true);
        animator.SetBool("Monster_anim|Idle_2", false);
        animator.SetBool("Monster_anim|Run", false); 
    }

    //make the boss run
    public void Run()
    {
        animator.SetBool("Monster_anim|Walk", false);
        animator.SetBool("Monster_anim|Idle_2", false);
        animator.SetBool("Monster_anim|Run", true); 

    }

    //make the boss do nothing, stand idle and EAT
    public void Idle()
    {
        animator.SetBool("Monster_anim|Walk", false);
        animator.SetBool("Monster_anim|Idle_2", true);
        animator.SetBool("Monster_anim|Run", false); 

    }
}
