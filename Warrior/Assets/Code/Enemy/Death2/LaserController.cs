using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    DeathReaperController deathReaperController;
    Rigidbody2D myBody;

    private bool callOnce;

    protected void Awake()
    {
        deathReaperController = FindObjectOfType<DeathReaperController>();
        myBody = GetComponent<Rigidbody2D>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("count");
            for (int i = 4; i >= 0; i--)
            {
                Destroy(deathReaperController.laserArray[i]);
            }
        }
    }
}
