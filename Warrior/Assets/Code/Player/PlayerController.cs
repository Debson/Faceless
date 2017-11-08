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

    protected void Update()
    {
        //Debug.Log(transform.position.y);
        if (Input.GetKey(KeyCode.LeftControl) && floorDetector.isTouchingFloor || EnterTerritory.IsCharacterControlEnabled && floorDetector.isTouchingFloor)
        {
            walkMovement.desiredWalkDirection = 0;
        }

        if((Input.GetButton("Left") && !Input.GetKey(KeyCode.LeftControl)) && 
            !EnterTerritory.IsCharacterControlEnabled)
        {
            walkMovement.desiredWalkDirection = Input.GetAxis("Left");
        }
        else if(Input.GetButtonUp("Left"))
        {
            walkMovement.desiredWalkDirection = 0;
        }

        if ((Input.GetButton("Right") && !Input.GetKey(KeyCode.LeftControl)) && 
            !EnterTerritory.IsCharacterControlEnabled)
        {
            walkMovement.desiredWalkDirection = Input.GetAxis("Right");
        }
        else if (Input.GetButtonUp("Right"))
        {
            walkMovement.desiredWalkDirection = 0;
        }

        if (Input.GetButtonDown("Attack") && !EnterTerritory.IsCharacterControlEnabled)
        {
            attackMovement.attackRequest = true;
        }
    }
}
