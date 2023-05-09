using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Rabbit : MonoBehaviour
{
    public enum State
    {
        Graze,
        GatherFood,
        Flee
    }
    [Header("Hunger Increment")]
    [SerializeField] private float hungerIncrement;
    [Header("Safe Zone Variables")]
    [SerializeField] public float safeZone;
    [Header("Rabbit Variables")]
    public float currentHunger;
    public float maxHunger;
    public float sightRadius;
    public float lifetime = 120f;
    public bool inHut;
    public bool goingToHut;
    public GameObject rabbitPrefab;
    public State currentState;
    public bool isBusy;
    [SerializeField] Animator rabbitAnimator;

    [SerializeField] public RabbitAnimation animator;

    [Header("Gather Food Variables")]
    public float foodReplenishment;
    public LayerMask vegetableLayer;

    [Header("Graze Variables")]
    public float grazeRadius;
    public float grazeDelay = 2f;

    private NavMeshAgent agent;
    private float lastMatingTime;
    private float birthTime;


    private Renderer renderer;
    private Collider collider;


    public Hut currentHut;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        birthTime = Time.time;
        StartCoroutine(Graze());
        currentState = State.Graze;
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
        StartCoroutine(LifetimeLimit());
        StartCoroutine(CheckForStuck());

    }

    void Update()
    {
        StateSwitch();
        IncrementHunger();
    }


    void IncrementHunger()
    {

        if (currentHunger >= maxHunger)
        {
            Destroy(gameObject);
        }

        currentHunger += hungerIncrement * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    void GatherFood()
    {
        GameObject closestVegetable = null;
        closestVegetable = FindNearestWithTag("Vegetable");

        if (closestVegetable != null)
        {
            agent.SetDestination(closestVegetable.transform.position);
            StartCoroutine(CheckIfVegetableExists(closestVegetable));
            isBusy = true;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vegetable"))
        {
            currentHunger -= foodReplenishment;
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
            Destroy(other.gameObject);
            isBusy = false;
        }
        else if (other.CompareTag("Hut") && goingToHut)
        {
            EnterHut();
        }
    }




    IEnumerator LifetimeLimit()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }


    IEnumerator Graze()
    {
        while (true)
        {
            yield return new WaitForSeconds(grazeDelay);

            if (currentState != State.Graze || isBusy) continue;

            Vector3 randomDirection = Random.insideUnitSphere * grazeRadius;
            randomDirection += transform.position;
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(randomDirection, out navMeshHit, grazeRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(navMeshHit.position);
            }
        }
    }

    void Flee()
    {
        GameObject nearestFox = FindNearestWithTag("Fox");

        if (nearestFox != null)
        {
            GameObject nearestHut = FindNearestWithTag("Hut");

            if (nearestHut != null)
            {
                agent.SetDestination(nearestHut.transform.position);
                goingToHut = true;
            }
            else
            {
                Vector3 fleeDirection = (transform.position - nearestFox.transform.position).normalized * sightRadius;
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(fleeDirection, out navMeshHit, sightRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(navMeshHit.position);
                }
            }
            isBusy = true;
        }
        else
        {
            isBusy = false;
        }
    }


    void EnterHut()
    {
        GameObject nearestHut = FindNearestWithTag("Hut");

        if (nearestHut != null)
        {
            //agent.enabled = false; 

            transform.SetParent(nearestHut.transform);
            transform.localPosition = Vector3.zero;

            renderer.enabled = false;
            collider.enabled = false;

            Hut hut = nearestHut.GetComponent<Hut>();
            if (hut != null)
            {
                currentHut = hut;
                hut.AddRabbit(this);
            }
        }
    }



    public void LeaveHut()
    {
        if (currentHut != null)
        {
            currentHut.RemoveRabbit(this);
            currentHut = null;
        }

        transform.SetParent(null);

        agent.enabled = true; // Re-enable the NavMeshAgent

        renderer.enabled = true;
        collider.enabled = true;
    }

    GameObject FindNearestWithTag(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        float minDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject obj in taggedObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    void StateSwitch()
    {
        GameObject nearestFox = FindNearestWithTag("Fox");
        bool foxInSight = nearestFox != null && Vector3.Distance(transform.position, nearestFox.transform.position) <= sightRadius;

        if (foxInSight && !isBusy)
        {
            currentState = State.Flee;
        }
        else if (currentHunger > 20 && !isBusy)
        {
            currentState = State.GatherFood;

        }
        else
        {
            currentState = State.Graze;
        }

        switch (currentState)
        {
            case State.Graze:
                animator.ChangeAnimationState(RabbitAnimation.State.Hopping);
                break;
            case State.GatherFood:
                GatherFood();
                animator.ChangeAnimationState(RabbitAnimation.State.Eating);
                break;
            case State.Flee:
                Flee();
                animator.ChangeAnimationState(RabbitAnimation.State.Escaping);
                break;
        }
    }

    bool IsFoxInSafeZone()
    {
        GameObject nearestFox = FindNearestWithTag("Fox");
        if (nearestFox != null)
        {
            float distance = Vector3.Distance(nearestFox.transform.position, transform.position);
            return distance < safeZone;
        }
        return false;
    }


    IEnumerator CheckIfVegetableExists(GameObject targetVegetable)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (targetVegetable == null)
            {
                isBusy = false;
                GatherFood();
                break;
            }
        }
    }

    IEnumerator CheckForStuck()
    {
        Vector3 previousPosition = transform.position;
        while (true)
        {
            yield return new WaitForSeconds(4f);
            if (!inHut && !isBusy && Vector3.Distance(previousPosition, transform.position) < 0.1f)
            {
                isBusy = false;
                currentState = State.Graze;
            }
            previousPosition = transform.position;
        }
    }

    public bool CanLeaveHut()
    {
        bool hungryEnough = currentHunger >= maxHunger * 0.6f;
        bool noFoxInSafeZone = !IsFoxInSafeZone(currentHut.transform);

        Debug.Log((currentHunger));

        return inHut && hungryEnough && noFoxInSafeZone;
    }


    bool IsFoxInSafeZone(Transform safeZoneCenter)
    {
        GameObject nearestFox = FindNearestWithTag("Fox");
        if (nearestFox != null)
        {
            float distance = Vector3.Distance(nearestFox.transform.position, safeZoneCenter.position);
            return distance < safeZone;
        }
        return false;
    }



}


