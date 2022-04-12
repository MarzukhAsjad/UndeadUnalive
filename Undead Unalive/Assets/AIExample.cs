using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class AIExample : MonoBehaviour {

    public enum WanderType {Random, Waypoint};


    public PlayerController pc;

<<<<<<< Updated upstream
=======
    public PlayerController pc;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    private Animator animator;
    private float loseTimer = 0;
    
=======
    private Animator animator;
    public Transform player;
    
>>>>>>> Stashed changes
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        renderer = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
        wanderPoint = RandomWanderPoint();
    }
    public void Update()
    {
<<<<<<< Updated upstream
        if (isAware) {
            print("chasing");
=======
        if (isAware)
        {
            agent.SetDestination(player.position);
>>>>>>> Stashed changes
            animator.SetBool("Aware", true);
            agent.SetDestination(pc.transform.position);
            if (!isDetecting)
            {
                loseTimer += Time.deltaTime;
                if (loseTimer >= loseThreshold)
                {
                    isAware = false;
                    loseTimer = 0;
                }
            }
            //renderer.material.color = Color.red;
        }
        else
        {
            print("searching for player");
            Wander();
            print("Wander");
            animator.SetBool("Aware", false);
            agent.speed = wanderSpeed;
            //renderer.material.color = Color.blue;
        }
        SearchForPlayer();
    }

    public void SearchForPlayer()
    {
<<<<<<< Updated upstream
        print("hello");
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(pc.transform.position)) < 120f)
        {
            print(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(pc.transform.position)));

            if (Vector3.Distance(pc.transform.position, transform.position) < viewDistance)
            {
                print(Vector3.Distance(pc.transform.position, transform.position));
                print("distance matches");
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
=======
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.position)) < fov / 2f)
        {
            if (Vector3.Distance(player.position, transform.position) < viewDistance)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, player.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                }
            }
>>>>>>> Stashed changes
        }
    }

    public void OnAware()
    {
        print("Aware entered");
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
            } else
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
