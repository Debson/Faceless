using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField]
    private int points = 500;

    [SerializeField]
    private AudioSource collectSound;

    AudioManager audioManager;


    protected void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioManager.coinSound.Play();
            ScoreManager.AddPoints(points);
            Destroy(gameObject);
        }
    }
}
