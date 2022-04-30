using System.Collections;
using Characters.Entity;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public NavMeshAgent agent; // navmesh 
    public int detectRadius; // zone for fast running
    public int health; // boss health
    public GameObject bossHealth;
    public GameObject bloodSpill;

    private GameObject player; // player reference
    private Rigidbody rb; // boss's rigidbody
    private float distance; // distance between boss and player
    private Animator animator; // boss's animator
    private Slider slider;
    public AudioSource eat;
    private bool _isBossDead = false;
    private bool isBossNear = false;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth.SetActive(isBossNear);
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        slider = bossHealth.GetComponent<Slider>();
        detectRadius = 20;
        health = 10;
        eat = GetComponent<AudioSource>();
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
            if (!_isBossDead)
                Death();
            _isBossDead = true;
        }

        if (distance < 100)
        {
            isBossNear = true;
            bossHealth.SetActive(isBossNear);
        }
        else
        {
            isBossNear = false;
            bossHealth.SetActive(isBossNear);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Vaccine") // to detect vaccine
        {
            Destroy(collider.gameObject);
            health -= 1;
            changeHealth();
        }

        if (collider.tag == "Player") // to detect player
        {
            agent.SetDestination(this.transform.position);
            Debug.Log("Player hit");
            Kill();
            eat.Play();
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
        agent.speed = 4.0f;
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
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("eat", true);
        // instantiate blood spill
        Instantiate(bloodSpill, transform.position, transform.rotation);
        // destroy player game object
        // instantiate blood spill\

        player.GetComponent<CharacterEntity>().ChangeHealth(0);
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
        ScoreManager.Instance.AddDeltaScore(100, "boss death");
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
        bossHealth.SetActive(false);
    }
}