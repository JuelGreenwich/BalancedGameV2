using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Currency : MonoBehaviour
{
    [SerializeField] int credits = 0;
    [SerializeField] TextMeshProUGUI playerCredits;

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
    void Update()
    {
        DisplayCredits();
    }
    void DisplayCredits()
    {
        playerCredits.text = credits.ToString();
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
