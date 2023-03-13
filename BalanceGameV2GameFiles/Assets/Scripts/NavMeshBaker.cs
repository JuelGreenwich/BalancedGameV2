using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    /*public NavMesh navMesh;
    public NavMeshData navMeshData;
    public NavMeshDataInstance navMeshDataInstance;

    private void Start()
    {
        // Create a NavMeshData object
        navMeshData = new NavMeshData();

        // Create a NavMeshDataInstance object
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);

        // Create a NavMeshBuilder object
        NavMeshBuildSettings buildSettings = NavMesh.GetSettingsByID(0); // Default build settings
        NavMeshBuilder builder = new NavMeshBuilder();

        // Define the NavMeshBuildSource object
        NavMeshBuildSource source = new NavMeshBuildSource();
        source.shape = NavMeshBuildSourceShape.Mesh;
        source.sourceObject = GetComponent<MeshFilter>().mesh;
        source.transform = transform.localToWorldMatrix;
        source.area = 0; // Default area

        // Build the NavMesh
        builder.BuildNavMeshData(buildSettings, new[] { source }, transform.localToWorldMatrix, Vector3.one, out NavMeshData navMeshData);

        // Assign the NavMeshData to the NavMeshDataInstance
        navMeshDataInstance.Remove();
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
        navMesh = navMeshDataInstance.navMesh;

        // Enable the NavMesh for runtime baking
        navMesh.dynamicNavMesh = true;
    }*/
}
