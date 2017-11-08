using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttackMovement : MonoBehaviour
{
    [SerializeField]
    public Collider2D attackTrigger;

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
        attackTrigger.enabled = false;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected void Update()
    {
        Debug.Log(isAttacking);

        if (attackRequest && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackCooldown;
            attackTrigger.enabled = true;
        }
        attackRequest = false;

        if (isAttacking)
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
