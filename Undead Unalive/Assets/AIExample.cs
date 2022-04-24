using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using UserInterface;
using Managers;

public class AIExample : MonoBehaviour
{

    public enum WanderType { Random, Waypoint };


    public PlayerController pc;
    public PlayerHUDController playerHUDController;
    public ParticleSystem ps;

    public WanderType wanderType = WanderType.Random;

    public float wanderSpeed = 4f;
    public float chaseSpeed = 7f;
    public float viewDistance = 10f;
    public float wanderRadius = 7f;
    public float loseThreshold = 10f;
    public Transform[] waypoints; //Array of waypoints is only used when waypoint wandering is selected

    private bool isAware = false;
    private bool isDetecting = false;
    private Vector3 wanderPoint;
    private NavMeshAgent agent;
    private Renderer renderer;
    private int waypointIndex = 0;
    private Animator animator;
    private float loseTimer = 0;
    public AudioSource source;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
        wanderPoint = RandomWanderPoint();
        ps = gameObject.GetComponentInChildren<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }
    public void Update()
    {
        
        if (isAware)
        {
            ps.Play();
            animator.SetBool("Aware", true);
            agent.SetDestination(pc.transform.position);
            if (!isDetecting)
            {
                loseTimer += Time.deltaTime;
                if (loseTimer >= loseThreshold)
                {
                    ps.Stop();
                    isAware = false;
                    loseTimer = 0;
                }
            }
            //renderer.material.color = Color.red;
        }
        else
        {
            if (Time.realtimeSinceStartup < 10)
            {
                source.volume = 0.0f;
            }
            else
            {
                source.volume = 1.0f;
                source.Play();
            }
            playerHUDController.EnemyNotifyPlayer(gameObject);
            Wander();
            animator.SetBool("Aware", false);
            agent.speed = wanderSpeed;
            //renderer.material.color = Color.blue;
        }
        SearchForPlayer();
    }

    public void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(pc.transform.position)) < 120f)
        {
            if (Vector3.Distance(pc.transform.position, transform.position) < viewDistance)
            {
                OnAware();
            }
            else
            {
                isDetecting = false;
            }
        }
        else
        {
            isDetecting = false;
        }
    }

    public void OnAware()
    {
        playerHUDController.EnemyNotifyPlayer(gameObject);
        isAware = true;
        isDetecting = false;
        loseTimer = 0;
    }

    public void Wander()
    {
        if (wanderType == WanderType.Random)
        {
            if (Vector3.Distance(transform.position, wanderPoint) < 2f)
            {
                wanderPoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderPoint);
            }
        }
        else
        {
            //Waypoint wandering
            if (waypoints.Length >= 2)
            {
                if (Vector3.Distance(waypoints[waypointIndex].position, transform.position) < 2f)
                {
                    if (waypointIndex == waypoints.Length - 1)
                    {
                        waypointIndex = 0;
                    }
                    else
                    {
                        waypointIndex++;
                    }
                }
                else
                {
                    agent.SetDestination(waypoints[waypointIndex].position);
                }
            }
            else
            {
                Debug.LogWarning("Please assign more than 1 waypoint to the AI: " + gameObject.name);
            }
        }
    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
}
