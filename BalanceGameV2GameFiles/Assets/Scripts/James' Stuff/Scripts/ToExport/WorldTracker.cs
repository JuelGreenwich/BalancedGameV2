using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTracker : MonoBehaviour
{
    public List<AnimalDisplay> itemsInGame = new List<AnimalDisplay>();

    public void AddToList(AnimalDisplay item)
    {
        itemsInGame.Add(item);
    }
}
