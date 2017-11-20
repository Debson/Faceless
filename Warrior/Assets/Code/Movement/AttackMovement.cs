using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackMovement : MonoBehaviour
{
    [SerializeField]
    private Collider2D attackTrigger;

    AudioManager audioManager;
    TurnAround turnAround;


    private bool currentState;

    public bool attackRequest
    {
        get; set;
    }

    private float attackTimer = 0;
    private float attackCooldown = 0.25f;


    public bool isAttacking
    {
        get; private set;
    }

    protected void Awake()
    {
        turnAround = GetComponent<TurnAround>();
        audioManager = FindObjectOfType<AudioManager>();
        attackTrigger.enabled = false;
    }

    protected void Update()
    {
        if (attackRequest && !isAttacking)
        {
            audioManager.attackSound[Random.Range(0, 2)].Play();
            isAttacking = true;
            attackTimer = attackCooldown;
            attackTrigger.enabled = true;
            currentState = turnAround.isFacingLeft;
        }
        attackRequest = false;

        if (isAttacking && (currentState == turnAround.isFacingLeft))
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                isAttacking = false;
                attackTrigger.enabled = false;
            }
        }
        else
        {
            isAttacking = false;
            attackTrigger.enabled = false;
        }
    }
}
