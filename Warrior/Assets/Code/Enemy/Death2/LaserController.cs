using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField]
    private int minDamageToGive;

    [SerializeField]
    private int maxDamageToGive;

    DeathReaperController deathReaperController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Rigidbody2D myBody;
    HealthManager healthManager;

    private bool callOnce;

    protected void Awake()
    {
        deathReaperController = FindObjectOfType<DeathReaperController>();
        hurtPlayerOnContact = FindObjectOfType<HurtPlayerOnContact>();
        myBody = GetComponent<Rigidbody2D>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            for (int i = 4; i >= 0; i--)
            {
                HurtPlayerOnContact.onAttackDamage += healthManager.AttackPlayer;
                Destroy(deathReaperController.laserArray[i]);
            }
        }
    }
}
