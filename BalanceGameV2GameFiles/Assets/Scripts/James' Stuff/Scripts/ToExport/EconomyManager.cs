using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public AnimalDisplay[] items;

    public Currency currency;

    
    void Start()
    {
        currency = FindObjectOfType<Currency>();
    }

    public void Purchase(int itemIndex)
    {
        if(currency.Credits >= items[itemIndex].cost)
        {
            currency.RemoveCredits(items[itemIndex].cost);
        }
        else
        {
            return;
        }
    }

    public void Testing123()
    {
        print("I am called");
    }
}
