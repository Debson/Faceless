using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    GameObject thingToSpawn;

    [SerializeField]
    float timeToSpawn = .5f;

    [SerializeField]
    public int spawnQuantity;

    protected void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        Instantiate(thingToSpawn, transform.position, Quaternion.identity);

        float sleepTime = timeToSpawn;
        yield return new WaitForSeconds(sleepTime);
    }
}
