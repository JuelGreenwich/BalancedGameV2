using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Fox : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Hunt
    }

    [Header("Hunger Increment")]
    [SerializeField] private float hungerIncrement;
    
    [Header("Fox Variables")]
    public float currentHunger;
    public float maxHunger;
    public float sightRadius;
    public float lifetime;
    public State currentState;
    public bool isBusy;
   [SerializeField] public FoxAnimation animator;

    [Header("Patrol Variables")]
    public float patrolRadius;
    public float patrolDelay = 2f;

    [Header("Hunt Variables")]
    public float foodReplenishment;
    

    public NavMeshAgent agent;
    private GameObject targetRabbit;

    private List<GameObject> huts;
    private float hutUpdateInterval = 1f;
    
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
        currentState = State.Patrol;
        UpdateHutInformation();
        StartCoroutine(LifeTimeLimit());
        StartCoroutine(UpdateHutInformationPeriodically());
    }

    void Update()
    {
        UpdateState();
        IncrementHunger();
    }

    void IncrementHunger()
    {
        currentHunger += hungerIncrement * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        
        if (currentHunger >=maxHunger)
        {
            Destroy(gameObject);
        }
    }
    
    
    IEnumerator LifeTimeLimit()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
    
    IEnumerator Patrol()
    {
        while (true)
        {
            yield return new WaitForSeconds(patrolDelay);

            if (currentState != State.Patrol || isBusy) continue;

            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(randomDirection, out navMeshHit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(navMeshHit.position);
            }
        }
    }

    void Hunt()
    {
        if (targetRabbit != null)
        {
            agent.SetDestination(targetRabbit.transform.position);
        }

        if (targetRabbit == null)
        {
            currentState = State.Patrol;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rabbit") && other.gameObject == targetRabbit)
        {
            currentHunger -= foodReplenishment;
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
            Destroy(other.gameObject);
            targetRabbit = null;
            isBusy = false;
        }
    }

    void UpdateState()
    {
        targetRabbit = FindNearestRabbit();

        bool targetRabbitInsideHut = false;
        if (targetRabbit != null)
        {
            foreach (GameObject hut in huts)
            {
                Collider hutCollider = hut.GetComponent<Collider>();
                if (hutCollider.bounds.Contains(targetRabbit.transform.position))
                {
                    targetRabbitInsideHut = true;
                    break;
                }
            }
        }

        NavMeshPath path = new NavMeshPath();
        bool pathExists = false;
        if (targetRabbit != null)
        {
            pathExists = NavMesh.CalculatePath(transform.position, targetRabbit.transform.position, NavMesh.AllAreas, path);
        }

        if (targetRabbit != null && !targetRabbitInsideHut && pathExists)
        {
            currentState = State.Hunt;
            Hunt();
            animator.ChangeAnimationState(FoxAnimation.State.Running);
        }
        else
        {
            currentState = State.Patrol;
            targetRabbit = null;
            animator.ChangeAnimationState(FoxAnimation.State.Walking);
        }
    }



    void UpdateHutInformation()
    {
        GameObject[] hutArray = GameObject.FindGameObjectsWithTag("Hut");
        huts = new List<GameObject>(hutArray);
    }
    
    IEnumerator UpdateHutInformationPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(hutUpdateInterval);
            UpdateHutInformation();
        }
    }
    
    
    GameObject FindNearestRabbit()
    {
        GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        float minDistance = Mathf.Infinity;
        GameObject closestRabbit = null;

        foreach (GameObject rabbit in rabbits)
        {
            float distance = Vector3.Distance(rabbit.transform.position, transform.position);
            if (distance < minDistance && distance <= sightRadius)
            {
                minDistance = distance;
                closestRabbit = rabbit;
            }
        }

        return closestRabbit;
    }
}
