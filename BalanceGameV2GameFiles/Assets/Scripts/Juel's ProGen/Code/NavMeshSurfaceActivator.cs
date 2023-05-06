using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceActivator : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public float delay = 5.0f;

    private void Start()
    {
        if (navMeshSurface == null)
        {
            Debug.LogWarning("No NavMeshSurface assigned to the NavMeshSurfaceActivator script.");
            return;
        }

        StartCoroutine(DelayedBakeNavMeshSurface());
    }

    private IEnumerator DelayedBakeNavMeshSurface()
    {
        yield return new WaitForSeconds(delay);
        navMeshSurface.BuildNavMesh();
    }
}