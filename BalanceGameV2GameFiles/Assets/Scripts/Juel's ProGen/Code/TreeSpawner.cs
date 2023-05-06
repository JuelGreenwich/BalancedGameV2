using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public int maxTrees = 10;
    public Vector3 spawnLocation;
    public Vector3 spawnAreaSize;
    public Color spawnLocationColor = Color.green;

    void Start()
    {
        int numTrees = Random.Range(0, maxTrees + 1);

        for (int i = 0; i < numTrees; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(spawnLocation.x - spawnAreaSize.x / 2f, spawnLocation.x + spawnAreaSize.x / 2f),
                spawnLocation.y,
                Random.Range(spawnLocation.z - spawnAreaSize.z / 2f, spawnLocation.z + spawnAreaSize.z / 2f)
            );
            RaycastHit hit;
            if (Physics.Raycast(spawnPos, Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    spawnPos = hit.point;
                }
            }
            Instantiate(treePrefab, spawnPos, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = spawnLocationColor;
        Gizmos.DrawWireCube(transform.position + spawnLocation, spawnAreaSize);
    }
}