using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Characters.Controller
{
    public class MobController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public GameObject safeHouse;
        public GameObject zombie;
        public int shieldRadius;
        public Vector3 offset; 

        private Camera mainCamera;
        private GameObject player;
        private Rigidbody rb;
        private bool enable;
        private float distance;
        private Animator animator;
        private Vector3 pos = new Vector3(204.29f, 0, 296.37f);
        private Vector3 desiredPosition; 
        
        public void Start()
        {
            safeHouse = new GameObject();
            safeHouse.transform.position = pos;
            

            agent = this.GetComponent<NavMeshAgent>();
            rb = this.GetComponent<Rigidbody>();
            animator = this.GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); 

            enable = true;
            shieldRadius = 0;
            offset = new Vector3(0, 0, 5); 

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //change the value of enable
                if (enable == true)
                {
                    enable = false; 
                } else {
                    enable = true; 
                }
            }

            /*if (Input.GetKeyDown(KeyCode.X))
            {
                agent.SetDestination(safeHouse.transform.position);
                Walk();

            }*/

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
                
                if (distance > shieldRadius) {

                    agent.SetDestination(desiredPosition);//player.transform.position);
                    Run(); 

                } else { //if distance is less than the shield radius, stop

                    agent.SetDestination(this.transform.position);
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
            Instantiate(zombie, transform.position, transform.rotation);
            Destroy(gameObject);
            
        }
    }
}
