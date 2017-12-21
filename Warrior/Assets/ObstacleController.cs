using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    private int minDamageToGive;

    [SerializeField]
    private int maxDamageToGive;

    WalkMovement walkMovement;
    AudioManager audioManager;

    private bool onlyOnce;

    protected void Awake()
    {
        walkMovement = FindObjectOfType<WalkMovement>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Debug.Log("dead");
            walkMovement.Knockback(collision.gameObject);
            walkMovement.knockbackStrength = 8f;
            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
            audioManager.playerHurt[Random.Range(0, 2)].Play();
        }
    }
}
