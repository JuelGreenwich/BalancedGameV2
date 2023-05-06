using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomss : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGeneraton levelGen;

    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (levelGen.stopGen == true)
        {
            int rand = Random.Range(0, levelGen.rooms.Length);
            Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
