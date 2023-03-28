using UnityEngine;
using UnityEngine.AI;

public class BakeNavMesh : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        }

        MarkNavigationStatic(gameObject);
        Bake();
        Debug.Log("NavMesh baked: " + CheckNavMeshBaked(transform.position));
    }

    public void MarkNavigationStatic(GameObject target)
    {
        if (target != null)
        {
            target.isStatic = true;
        }
        else
        {
            Debug.LogError("Target GameObject is null.");
        }
    }

    public void Bake()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("No NavMeshSurface component found.");
            return;
        }

        navMeshSurface.BuildNavMesh();
    }

    public bool CheckNavMeshBaked(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas);
    }
}