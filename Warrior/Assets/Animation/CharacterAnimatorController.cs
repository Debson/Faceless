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

        animator.SetBool("Idle", Mathf.Abs(myBody.velocity.x) == 0 ? true: false);
        animator.SetFloat("Speed", Mathf.Abs(myBody.velocity.x));
        animator.SetBool("Jump", !floorDetector.isTouchingFloor);
        animator.SetFloat("JumpSpeed", Mathf.Abs(myBody.velocity.y));
        animator.SetFloat("JumpSpeedRaw", myBody.velocity.y);
        //animator.SetBool("isCrouching", crouchMovement.isCrouching);
        animator.SetBool("Dash", dashMovement.dashActive);
    }
}
