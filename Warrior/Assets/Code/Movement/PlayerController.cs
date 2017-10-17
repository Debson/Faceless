using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WalkMovement))]
[RequireComponent(typeof(JumpMovement))]
[RequireComponent(typeof(AttackMovement))]
public class PlayerController : MonoBehaviour
{
    WalkMovement walkMovement;
    JumpMovement jumpMovement;
    AttackMovement attackMovement;
    CrouchMovement crouchMovement;

    protected void Awake()
    {
        walkMovement = GetComponent<WalkMovement>();
        jumpMovement = GetComponent<JumpMovement>();
        attackMovement = GetComponent<AttackMovement>();
        crouchMovement = GetComponent<CrouchMovement>();
    }

    protected void FixedUpdate()
    {
        walkMovement.desiredWalkDirection = Input.GetAxis("Horizontal");
    }

    protected void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            jumpMovement.jumpRequest = true;
        }

        if(Input.GetButtonDown("Attack"))
        {
            attackMovement.attackRequest = true;
        }

       /* if(Input.GetButton("Crouch"))
        {
            crouchMovement.crouchRequest = true;
        }*/

    }


}
