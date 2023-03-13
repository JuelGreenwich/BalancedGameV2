using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{
    public GameObject meshObject;
    public GameObject navMeshObject;

    void Start()
    {
        //BuildNavMesh();
    }
    /*
    void BuildNavMesh()
    {
        Mesh mesh = meshObject.GetComponent<MeshFilter>().mesh;
        NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(new NavMeshBuildSettings(), mesh);

        if (navMeshData != null)
        {
            NavMesh navMesh = navMeshObject.AddComponent<NavMesh>();
            NavMeshDataInstance navMeshInstance = new NavMeshDataInstance(navMeshData);
            navMesh.AddNavMeshData(navMeshInstance);
            Debug.Log("NavMesh added successfully.");
        }
        else
        {
            Debug.LogError("Failed to build NavMesh data.");
        }
    }*/
}
