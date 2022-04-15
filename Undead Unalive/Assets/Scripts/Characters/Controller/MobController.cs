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
        public GameObject player;
        public Rigidbody rb;

        //get the position of the safe house

        private bool enable;
        private float distance; 

        public void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            enable = true;
            rb = this.GetComponent<Rigidbody>(); 

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //change the value of enable
                if (enable == true)
                {
                    enable = false; 
                } else
                {
                    enable = true; 
                }
            }

            if (!enable) //if disabled, mob should return to the safe house
            {
                agent.SetDestination(safeHouse.transform.position); 

            } else { //mob should follow player

                distance = Vector3.Distance(player.transform.position, this.transform.position); 
                
                if(distance > 1)
                {
                    agent.SetDestination(player.transform.position); 

                }
                else //if distance is less than 1, stop
                {
                    rb.velocity = new Vector3(0, 0, 0); 
                }

            }
       
        }


    }
}
