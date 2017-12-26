using UnityEngine;

[RequireComponent(typeof(WalkMovement))]
[RequireComponent(typeof(JumpMovement))]
[RequireComponent(typeof(AttackMovement))]
[RequireComponent(typeof(TurnAround))]
[RequireComponent(typeof(FloorDetector))]
public class PlayerController : MonoBehaviour
{
    WalkMovement walkMovement;
    JumpMovement jumpMovement;
    AttackMovement attackMovement;
    FloorDetector floorDetector;
    Rigidbody2D myBody;
    DashMovement dashMovement;

    public bool CharacterControlDisabled { get; set; }

    protected void Awake()
    {
        walkMovement = GetComponent<WalkMovement>();
        jumpMovement = GetComponent<JumpMovement>();
        attackMovement = GetComponent<AttackMovement>();
        floorDetector = GetComponent<FloorDetector>();
        myBody = GetComponent<Rigidbody2D>();
        dashMovement = GetComponent<DashMovement>();
        CharacterControlDisabled = false;
    }

    protected void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && floorDetector.isTouchingFloor || CharacterControlDisabled && floorDetector.isTouchingFloor)
        {
            walkMovement.desiredWalkDirection = 0;
        }

        if((Input.GetButton("Left") && !Input.GetKey(KeyCode.LeftControl)) && 
            !CharacterControlDisabled)
        {
            walkMovement.desiredWalkDirection = Input.GetAxis("Left");
        }
        else if(Input.GetButtonUp("Left"))
        {
            walkMovement.desiredWalkDirection = 0;
        }

        if ((Input.GetButton("Right") && !Input.GetKey(KeyCode.LeftControl)) && 
            !CharacterControlDisabled)
        {
            walkMovement.desiredWalkDirection = Input.GetAxis("Right");
        }
        else if (Input.GetButtonUp("Right"))
        {
            walkMovement.desiredWalkDirection = 0;
        }

        /*if (Input.GetButtonDown("Attack") && !CharacterControlEnabled)
        {
            attackMovement.attackRequest = true;
        }*/

        if(Input.GetKeyDown(KeyCode.D) && floorDetector.isTouchingFloor && !CharacterControlDisabled)
        {
            dashMovement.dash = true;
        }
    }
}
