using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    // Variables to store the initial values for life span, hunger rate, max hunger, roam radius, hunger threshold, mating radius, max children, and AI prefab
    public float lifeSpan = 15f;
    public float hungerRate = 0.1f;
    public float maxHunger = 1f;
    public float roamRadius = 5f;
    public float hungerThreshold = 2f;
    [SerializeField] public float matingRadius = 5f;
    public int maxChildren = 3;
    public GameObject aiPrefab;

    // Variables to store the current hunger and children counter, and whether the AI is mating or not
    [SerializeField] private float currentHunger = 0f;
    [SerializeField] private int childrenCounter = 0;
    [SerializeField] private bool isMating = false;
    [SerializeField] private GameObject matingTarget = null;

    // Variables to store the current life and the NavMeshAgent component
    private float currentLife;
    private NavMeshAgent agent;
    private GameObject nearestFood;

    // Variable to store the mating cooldown time
    public float matingCoolDown = 5f;
    private float matingCoolDownTimer = 0f;

    void Start()
    {
        // Initialise the current life and NavMeshAgent component
        currentLife = lifeSpan;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Decrement life, check hunger, move towards nearest food, check for mating and perform mating
        DecrementLife();
        CheckHunger();
        MoveToNearestFood();
        matingCoolDownTimer += Time.deltaTime;
        if (matingCoolDownTimer >= matingCoolDown)
        {
            CheckForMating();
        }

        Mating();
        if (isMating == false)
        {
            Roam();
        }
    }

    // Function to decrement the AI's life
    private void DecrementLife()
    {
        currentLife -= Time.deltaTime;
        if (currentLife <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // Function to check the AI's hunger
    private void CheckHunger()
    {
        currentHunger += hungerRate * Time.deltaTime;
        if (currentHunger >= maxHunger)
        {
            Destroy(gameObject);
        }
    }

    // Function to move the AI towards the nearest food
    private void MoveToNearestFood()
    {
        if (currentHunger >= hungerThreshold)
        {
            if (nearestFood == null)
            {
                nearestFood = FindNearestFood();
            }
            else
            {
                agent.destination = nearestFood.transform.position;
            }
        }
    }

    // Function to detect collision with food and consume it
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vegetable"))
        {
            currentHunger = 0f;
            Destroy(other.gameObject);
        }
    }

    // Function to find the nearest food
    GameObject FindNearestFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Vegetable");
        if (foods.Length == 0)
        {
            return null;
        }

        GameObject nearest = null;
        float minDistance = float.MaxValue;
        foreach (GameObject food in foods)
        {
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = food;
            }
        }

        return nearest;
    }

// Function to make the AI roam around
    void Roam()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (agents.Length > 1)
        {
            GameObject closest = null;
            float minDistance = float.MaxValue;
            foreach (GameObject agent in agents)
            {
                if (agent == gameObject)
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, agent.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = agent;
                }
            }

            if (closest != null)
            {
                agent.destination = closest.transform.position;
            }
        }
        else
        {
            Vector3 target = transform.position + Random.insideUnitSphere * roamRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target, out hit, roamRadius, 1))
            {
                agent.destination = hit.position;
            }
        }
    }

// Function to check if the AI can mate with another AI
    bool CheckForMating()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (agents.Length > 1)
        {
            GameObject closest = null;
            float minDistance = float.MaxValue;
            foreach (GameObject agent in agents)
            {
                if (agent == gameObject)
                {
                    continue;
                }

                BasicAI ai = agent.GetComponent<BasicAI>();
                if (!ai.isMating)
                {
                    float distance = Vector3.Distance(transform.position, agent.transform.position);
                    if (distance <= matingRadius && currentHunger < hungerThreshold &&
                        ai.currentHunger < hungerThreshold && childrenCounter < maxChildren &&
                        ai.childrenCounter < ai.maxChildren)
                    {
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closest = agent;
                        }
                    }
                }
            }

            if (closest != null)
            {
                BasicAI ai = closest.GetComponent<BasicAI>();
                isMating = true;
                ai.isMating = true;
                matingTarget = closest;
                ai.matingTarget = gameObject;
                return true;
            }
        }

        return false;
    }
// Mating Function

    private void Mating()
    {
        if (matingTarget != null)
        {
// Create a new AI, set its values and increment the children counter for both AIs
            GameObject newAI = Instantiate(aiPrefab, transform.position, Quaternion.identity);
            BasicAI newAIScript = newAI.GetComponent<BasicAI>();
            newAIScript.lifeSpan = lifeSpan;
            newAIScript.hungerRate = hungerRate;
            newAIScript.maxHunger = maxHunger;
            newAIScript.roamRadius = roamRadius;
            newAIScript.hungerThreshold = hungerThreshold;
            newAIScript.matingRadius = matingRadius;
            newAIScript.maxChildren = maxChildren;
            newAIScript.aiPrefab = aiPrefab;
            newAIScript.currentHunger = 0f;
            newAIScript.childrenCounter = 0;
            newAIScript.matingCoolDownTimer = 0f;
            childrenCounter++;
            BasicAI ai = matingTarget.GetComponent<BasicAI>();
            ai.childrenCounter++;
            // Reset the mating status and mating target for both AIs
            isMating = false;
            ai.isMating = false;
            matingTarget = null;
            ai.matingTarget = null;
        }
    }
}
            