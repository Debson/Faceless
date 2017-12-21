using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerFallScript : MonoBehaviour
{
    CameraFollow cameraFollow;
    HealthManager healthManager;

    protected void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    protected void Update ()
    {
		
	}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CameraFollow.onPlayerFall += cameraFollow.PlayerFall;
            HealthManager.onDeath += healthManager.Death;
        }
    }
}
