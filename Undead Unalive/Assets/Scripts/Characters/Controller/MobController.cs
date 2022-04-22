﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Characters.Controller
{
    public class MobController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public GameObject safeHouse;
        public int shieldRadius; // when should the mob shift to walking
        public int detectRadius; // when should the mob shift to running


        private GameObject player;
        private Rigidbody rb;
        private bool enable;
        private float distance;
        private Animator animator;
        private Vector3 pos = new Vector3(204.29f, 0, 296.37f);
        
        public void Start()
        {
            safeHouse = new GameObject();
            safeHouse.transform.position = pos; 

            agent = this.GetComponent<NavMeshAgent>();
            rb = this.GetComponent<Rigidbody>();
            animator = this.GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");

            shieldRadius = 4;
            detectRadius = 10;

            enable = true;

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
                
                if (distance > detectRadius) {
                   
                    agent.SetDestination(player.transform.position);
                    agent.speed = 3.5f;
                    Run(); 

                } 
                else if (distance > shieldRadius)
                {
                    agent.SetDestination(player.transform.position);
                    agent.speed = 2.0f;
                    Walk();
                }
                else { //if distance is less than the shield radius, stop

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
    }
}
