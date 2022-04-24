using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Characters.Controller
{
    public class MobController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public int shieldRadius;
        public int detectRadius;
        public Vector3 offset;
        private GameObject player;
        private Rigidbody rb;
        private bool enable;
        private float distance;
        private Animator animator;
        private Camera mainCamera;
        private Vector3 desiredPosition;
        private float creationTime = 5.0f;

        public void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            rb = this.GetComponent<Rigidbody>();
            animator = this.GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            enable = true;
            shieldRadius = 4;
            detectRadius = 10;
            offset = new Vector3(0, 0, 5);
        }

        public void Update()
        {

            creationTime -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                //change the value of enable
                if (enable == true)
                {
                    enable = false;
                }
                else
                {
                    enable = true;
                }
            }

            //rotating the offset
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * offset;


            //if disabled, mob should just stay at there current position
            if (!enable)
            {
                agent.SetDestination(this.transform.position);
                rb.velocity = new Vector3(0, 0, 0);
                Idle();
            }

            else //mob should follow player
            {
                distance = Vector3.Distance(player.transform.position, this.transform.position);
                desiredPosition = player.transform.position + offset;

                agent.SetDestination(desiredPosition);
                if (distance > detectRadius)
                {// if the mob is > detectRadius away from player, they'll run
                    agent.speed = 3.5f;
                    Run();
                }
                else if (distance > shieldRadius) //otherwise they'll walk
                {
                    agent.speed = 2;
                    Walk();
                }
                else // otherwise they'll stand
                {
                    agent.SetDestination(this.transform.position);
                    agent.speed = 0;
                    rb.velocity = new Vector3(0, 0, 0);
                    Idle();
                }
            }

        }

        //make the survivor walk
        public void Walk()
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
            animator.SetBool("SprintJump", false);
        }

        //make the survivor run
        public void Run()
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", false);
            animator.SetBool("SprintJump", true);
        }

        //make the survivor do nothing, stand idle
        public void Idle()
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            animator.SetBool("SprintJump", false);

        }

        private void OnParticleCollision(GameObject other)
        {
            Debug.Log(other.tag);
            if (creationTime <= 0.0f && other.CompareTag("ToxicGas"))
            {
                Instantiate(GameObject.FindGameObjectWithTag("Zombie"), transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
