using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{

    [SerializeField]
    private int health = 66;

    AudioManager audioManager;

    

    protected void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioManager.healSound.Play();
            HealthManager.AddHealth(health);
            Destroy(gameObject);
        }
    }

}
