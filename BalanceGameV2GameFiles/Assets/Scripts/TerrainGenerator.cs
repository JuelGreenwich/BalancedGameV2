using UnityEngine;
using UnityEngine.AI;

public class TerrainGenerator : MonoBehaviour
{
    /*public int terrainWidth = 200;
    public int terrainLength = 200;
    public float minHeight = 0f;
    public float maxHeight = 20f;

    private NavMeshData navMeshData;
    private Terrain terrain;

    private void Start()
    {
        // Generate the terrain
        terrain = Terrain.activeTerrain;
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Create a NavMeshData object
        navMeshData = new NavMeshData();

        // Generate the NavMesh
        NavMeshBuildSettings settings = NavMesh.GetSettingsByID(0);
        NavMeshSource navMeshSource = new NavMeshSource();
        navMeshSource.transform = terrain.transform.localToWorldMatrix;
        navMeshSource.shape = NavMeshSourceShape.Terrain;
        navMeshSource.sourceObject = terrain.terrainData;
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh(new NavMeshBuildSource[] { navMeshSource }, terrain.transform.localToWorldMatrix, settings, navMeshData);

        // Assign the NavMesh to agents
        NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent agent in agents)
        {
            agent.navMeshData = navMeshData;
        }
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = terrainWidth + 1;
        terrainData.size = new Vector3(terrainWidth, maxHeight, terrainLength);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[terrainWidth, terrainLength];
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int z = 0; z < terrainLength; z++)
            {
                float height = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * maxHeight;
                heights[x, z] = height;
            }
        }
        return heights;
    }*/
}
