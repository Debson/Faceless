using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpMovement : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed = 7f;

    [SerializeField]
    private float secondJumpSpeed = 10f;

    [SerializeField]
    private float doubleJumpDuration = 0.5f;

    public bool isDoubleJump { get; private set; }

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
            animator.SetBool("Double_Jump", true);
            StartCoroutine(DoubleJumpDuration());

            if (myBody.velocity.y > 0)
            {
                Jump(secondJumpSpeed);
            }

            if (myBody.velocity.y < 0)
            {
                Jump(secondJumpSpeed * 3.3f);
            }

            isDoubleJump = true;
        }
    }

    public void Jump(float speed)
    {
        //audioManager.playerJump[Random.Range(0, 4)].Play();
        myBody.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
    }

    IEnumerator DoubleJumpDuration()
    {
        yield return new WaitForSeconds(doubleJumpDuration);
        animator.SetBool("Double_Jump", false);
    }
}
