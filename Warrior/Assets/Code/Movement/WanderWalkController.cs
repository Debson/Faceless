using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WanderWalkController : MonoBehaviour
{

    [SerializeField]
    float timeBeforeFirstWander = 10;

    [SerializeField]
    float minTimeBetweenReconsideringDirection = 1;

    [SerializeField]
    float maxTimeBetweenReconsideringDirection = 10;

    WalkMovement walkMovement;
    BatMovement batMovement;

    protected void Awake()
    {
        walkMovement = GetComponent<WalkMovement>();
        batMovement = GetComponent<BatMovement>();
    }


    protected void Start()
    {
        if (!batMovement.playerInRange)
        {
            StartCoroutine(Wander());
        }
    }

    IEnumerator Wander()
    {
        walkMovement.desiredWalkDirection = 1;

        if (timeBeforeFirstWander > 0)
        {
            float timeToSleep = timeBeforeFirstWander + GetRandomTimeToSleep();
            yield return new WaitForSeconds(timeToSleep);
        }

        while (true)
        {
            SelectARandomWalkDirection();
            float timeToSleep = GetRandomTimeToSleep();
            yield return new WaitForSeconds(timeToSleep);
        }
    }

    void SelectARandomWalkDirection()
    {
        walkMovement.desiredWalkDirection
          = UnityEngine.Random.value <= .5f ? 1 : -1;
    }

    float GetRandomTimeToSleep()
    {
        return UnityEngine.Random.Range(
          minTimeBetweenReconsideringDirection,
          maxTimeBetweenReconsideringDirection);
    }
}

