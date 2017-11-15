using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    HurtPlayerOnContact hurtPlayerOnContact;
    FloorDetector floorDetector;
    Animator animator;


    protected void Awake()
    {
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        floorDetector = FindObjectOfType<FloorDetector>();
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        Debug.Log(hurtPlayerOnContact.isAttacking);
        animator.SetBool("isAttacking", hurtPlayerOnContact.attackingAnimation);
        animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
        animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);
    }
}
