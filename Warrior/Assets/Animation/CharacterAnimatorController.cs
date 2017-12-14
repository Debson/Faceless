using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterAnimatorController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D myBody;
    FloorDetector floorDetector;
    AttackMovement attackMovement;
    CrouchMovement crouchMovement;
    DashMovement dashMovement;

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        floorDetector = GetComponentInChildren<FloorDetector>();
        attackMovement = GetComponent<AttackMovement>();
        crouchMovement = GetComponent<CrouchMovement>();
        dashMovement = GetComponent<DashMovement>();
    }

    protected void LateUpdate()
    {
        animator.SetFloat("Speed", myBody.velocity.magnitude);
        animator.SetBool("Jump", !floorDetector.isTouchingFloor);
        //animator.SetBool("isCrouching", crouchMovement.isCrouching);
        animator.SetBool("Dash", dashMovement.dashActive);
    }
}
