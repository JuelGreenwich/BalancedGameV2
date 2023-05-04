using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] int credits = 0;

    public int Credits
    {
        get
        {
            return credits;
        }
    }


    void Start()
    {
        credits = 50;
    }

    public void RegenCredits()
    {
        credits = 50;
    }

    public void RemoveCredits(int amount)
    {
        credits -= amount;
    }
}
