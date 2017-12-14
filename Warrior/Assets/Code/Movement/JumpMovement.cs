using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpMovement : MonoBehaviour
{
    public bool doubleJump;
    public bool jumpRequest;
    public bool isDoubleJump
    {
        get; private set;
    }

    [SerializeField]
    float jumpSpeed = 7f;

    [SerializeField]
    float secondJumpSpeed = 10f;

    PlayerController playerController;
    Rigidbody2D myBody;
    FloorDetector floorDetector;
    CrouchMovement crouchMovement;
    Physics2D gravity;
    AudioManager audioManager;
    Animator animator;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        floorDetector = GetComponent<FloorDetector>();
        playerController = GetComponent<PlayerController>();
        crouchMovement = GetComponent<CrouchMovement>();
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        if (floorDetector.isTouchingFloor)
        {
            isDoubleJump = false;
        }

        if (Input.GetButtonDown("Jump") && floorDetector.isTouchingFloor && !crouchMovement.isCrouching && !playerController.CharacterControlEnabled)
        {
            Jump(jumpSpeed);
        }

        if (Input.GetButtonDown("Jump") && !floorDetector.isTouchingFloor && !isDoubleJump)
        {
            if (myBody.velocity.y > 0)
            {
                Jump(secondJumpSpeed);
            }

            if (myBody.velocity.y < 0)
            {
                Jump(secondJumpSpeed * 3.14159f);
                doubleJump = true;
            }

            isDoubleJump = true;
        }

    }

    public void Jump(float speed)
    {
        //audioManager.playerJump[Random.Range(0, 4)].Play();
        myBody.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
    }
}
