using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WalkMovement))]
[RequireComponent(typeof(JumpMovement))]
[RequireComponent(typeof(AttackMovement))]
[RequireComponent(typeof(TurnAround))]
public class PlayerController : MonoBehaviour
{
    WalkMovement walkMovement;
    JumpMovement jumpMovement;
    AttackMovement attackMovement;
    FloorDetector floorDetector;


    protected void Awake()
    {
        walkMovement = GetComponent<WalkMovement>();
        jumpMovement = GetComponent<JumpMovement>();
        attackMovement = GetComponent<AttackMovement>();
        floorDetector = GetComponent<FloorDetector>();

    }

  /*  protected void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            walkMovement.desiredWalkDirection = Input.GetAxis("Horizontal");
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            TurnAround.IsFacingLeft2 = true;

            //Debug.Log(TurnAround.IsFacingLeft2);
        }
    }*/

    protected void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl) && floorDetector.isTouchingFloor)
        {
            walkMovement.desiredWalkDirection = 0;
        }

        /*if (!Input.GetKey(KeyCode.LeftControl) || !floorDetector.isTouchingFloor)
        {
            walkMovement.desiredWalkDirection = Input.GetAxis("Horizontal");
        }*/

        if(Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.LeftControl) || 
            Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftControl))
        {
            walkMovement.desiredWalkDirection = -1;
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            walkMovement.desiredWalkDirection = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftControl) || 
            Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftControl))
        {
            walkMovement.desiredWalkDirection = 1;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            walkMovement.desiredWalkDirection = 0;
        }




        if (Input.GetButtonDown("Attack"))
        {
            attackMovement.attackRequest = true;
        }

       /* if(Input.GetButton("Crouch"))
        {
            crouchMovement.crouchRequest = true;
        }*/

    }


}
