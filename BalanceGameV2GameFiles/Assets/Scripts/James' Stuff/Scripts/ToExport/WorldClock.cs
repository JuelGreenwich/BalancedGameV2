using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldClock : MonoBehaviour
{
    public int gameTime;
    public float tickRate = 1f;

    public int dayCounter = 0;
    bool isPaused = false;

    Currency currency;

    #region Get and Set variables
    public int GameTime
    {
        get
        {
            return gameTime;
        }
    }

    public float TickRate
    {
        get
        {
            return tickRate;
        }
        set
        {
            tickRate = value;
        }
    }

    public int DayCounter
    {
        get
        {
            return dayCounter;
        }
    }

    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            isPaused = value;
        }
    }
    #endregion

    #region World Clock 1
    void Awake()
    {
        StartCoroutine(WorldTick());
    }

    void Start()
    {
        currency = FindObjectOfType<Currency>();    
    }

    public IEnumerator WorldTick()
    {
        while (!isPaused)
        {
            if (gameTime < 10)
            {
                gameTime++;
            }
            else
            {
                dayCounter++;
                gameTime = 0;
                currency.RegenCredits();
            }

            yield return new WaitForSeconds(tickRate);
        }
    }
    #endregion
}
