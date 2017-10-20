using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int startingLives;
    private int lifeCounter;

    protected void Start()
    {
        lifeCounter = startingLives;
    }

    protected void Update()
    {
        if(lifeCounter < 0)
        {
            // Game over
        }
    }

    public void GiveLife()
    {
        lifeCounter++;
    }

    public void TakeLife()
    {
        lifeCounter--;
    }
}
