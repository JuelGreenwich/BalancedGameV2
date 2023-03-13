using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public int Jump;

    void Start()
    { 
        Jump = Random.Range(0, 100);
        Debug.Log(Jump);
    }
}
