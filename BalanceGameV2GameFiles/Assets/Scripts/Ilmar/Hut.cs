using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Hut : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private float safeZone;
    private List<Rabbit> rabbitsInHut = new List<Rabbit>();
    private float lastSpawnTime = 0f;

    void Start()
    {
        GameObject rabbitObject = GameObject.FindGameObjectWithTag("Rabbit");
        if (rabbitObject != null)
        {
            Rabbit rabbit = rabbitObject.GetComponent<Rabbit>();
            if (rabbit != null)
            {
                safeZone = rabbit.safeZone;
            }
        }
    }

    private void Update()
    {
        SpawnRabbits();
        CheckIfRabbitShouldLeave();
        PushFoxesAway();
    }

    public void AddRabbit(Rabbit rabbit)
    {
        rabbitsInHut.Add(rabbit);
    }

    public void RemoveRabbit(Rabbit rabbit)
    {
        rabbitsInHut.Remove(rabbit);
    }

    private void SpawnRabbits()
    {
        if (Time.time - lastSpawnTime >= spawnInterval && rabbitsInHut.Count >= 2)
        {
            SpawnRabbit();
        }
    }

    private void SpawnRabbit()
    {
        if (rabbitPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            GameObject newRabbit = Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity, transform.parent);

            lastSpawnTime = Time.time;
        }
    }

    private void CheckIfRabbitShouldLeave()
    {
        foreach (Rabbit rabbit in rabbitsInHut)
        {
            rabbit.inHut = true;
            Debug.Log(rabbit.CanLeaveHut());
            if (rabbit.CanLeaveHut())
            {
                Debug.Log(rabbitsInHut);
                rabbit.LeaveHut();
                RemoveRabbit(rabbit);
                rabbit.inHut = false;

                break;
            }
        }
    }
    
    private void PushFoxesAway()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, (safeZone));
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Fox"))
            {
                Fox fox = hitCollider.GetComponent<Fox>();
                if (fox != null && fox.currentState != Fox.State.Patrol)
                {
                    fox.currentState = Fox.State.Patrol;
                    Vector3 randomDirection = Random.insideUnitSphere * fox.patrolRadius;
                    randomDirection += hitCollider.transform.position;
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition(randomDirection, out navMeshHit, fox.patrolRadius, NavMesh.AllAreas))
                    {
                        fox.agent.SetDestination(navMeshHit.position);
                    }
                }
            }
        }
    }




}