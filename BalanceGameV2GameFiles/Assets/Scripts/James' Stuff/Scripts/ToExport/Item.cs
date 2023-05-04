using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string name;
    public int cost;
    public int health;
    public int index;
    public GameObject mesh;


    public void SaveItem()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadItem()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        cost = data.cost;
        index = data.health;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;
    }
}


