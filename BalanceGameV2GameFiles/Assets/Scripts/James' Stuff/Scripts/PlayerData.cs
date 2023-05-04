using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public int cost;
    public int health;
    public float[] position;

    public PlayerData(Item item)
    {
        cost = item.cost;
        health = item.health;

        position = new float[3];
        position[0] = item.transform.position.x;
        position[1] = item.transform.position.y;
        position[2] = item.transform.position.z;
    }
}
