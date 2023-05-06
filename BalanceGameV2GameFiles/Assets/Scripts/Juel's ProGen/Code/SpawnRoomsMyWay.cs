using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomsMyWay : MonoBehaviour
{
    public GameObject objToDestroy;
    //public GameObject cubeDestroy;
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Room")
        {
            Destroy(objToDestroy);
            //Destroy(cubeDestroy);
        }
    }
}
