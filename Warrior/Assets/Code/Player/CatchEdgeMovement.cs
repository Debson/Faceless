using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchEdgeMovement : MonoBehaviour
{
    [SerializeField]
    GameObject catchCollider;

    [SerializeField]
    Transform chest;

    [SerializeField]
    Transform hand;

    [SerializeField]
    LayerMask layers;

    PlayerController player;
    TurnAround turnAround;
    FloorDetector floorDetector;
    Animator animator;
    AttackMovement attackMovement;
    WalkMovement walkMovement;
    AudioManager audioManager;

    private Coroutine lastRoutine = null;
    private Vector2 startPos;

    private float direction;

    private bool enableCatchMovement;
    private bool isHoldingEdge;
    private bool climbingInProgress;
    private bool playonce;

    protected void Awake()
    {
        player = GetComponent<PlayerController>();
        turnAround = GetComponent<TurnAround>();
        floorDetector = GetComponent<FloorDetector>();
        animator = GetComponent<Animator>();
        startPos = catchCollider.transform.localPosition;
        attackMovement = GetComponent<AttackMovement>();
        walkMovement = GetComponent<WalkMovement>();
        audioManager = FindObjectOfType<AudioManager>();
        enableCatchMovement = true;
    }

    protected void Update()
    {
        CatchingEdge();
        animator.SetBool("Edge_Catch", isHoldingEdge);
    }

    private void CatchingEdge()
    {
        if (!floorDetector.isTouchingFloor)
        {
            RaycastHit2D objectInFront;

            if (turnAround.isFacingLeft)
            {
                objectInFront = Physics2D.Raycast(chest.position, Vector2.left, 1.5f, layers);
                Debug.DrawRay(chest.position, new Vector2(-1.5f, 0), Color.yellow);
                direction = -1;
            }
            else
            {
                objectInFront = Physics2D.Raycast(chest.position, Vector2.right, 1.5f, layers);
                Debug.DrawRay(chest.position, new Vector2(1.5f, 0), Color.yellow);
                direction = 1;
            }


            if (objectInFront.collider != null)
            {
                Debug.DrawRay(catchCollider.transform.position, new Vector2(0, -1), Color.red);
                RaycastHit2D objectUnder = Physics2D.Raycast(catchCollider.transform.position, Vector2.down, 1f, layers);

                if (objectUnder.collider != null && enableCatchMovement && !climbingInProgress)
                {
                    attackMovement.AttackMovementEnabled = false;
                    if (!playonce)
                    {
                        //audioManager.playerDash[3].Play();
                        playonce = true;
                    }

                    catchCollider.GetComponent<Collider2D>().enabled = true;
                    animator.SetBool("Edge_Catch", true);
                    isHoldingEdge = true;

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        catchCollider.GetComponent<Collider2D>().enabled = false;
                        enableCatchMovement = false;
                        isHoldingEdge = false;
                    }

                    if (Input.GetKeyDown(KeyCode.LeftArrow) && turnAround.isFacingLeft || Input.GetKeyDown(KeyCode.RightArrow) && !turnAround.isFacingLeft)
                    {
                        animator.SetBool("Climb_Up", true);
                        climbingInProgress = true;
                        lastRoutine = StartCoroutine(ClimbUp());
                    }
                }
                else if (!climbingInProgress)
                {
                    catchCollider.GetComponent<Collider2D>().enabled = false;
                    isHoldingEdge = false;
                }
            }
            else if (!climbingInProgress)
            {
                catchCollider.GetComponent<Collider2D>().enabled = false;
                isHoldingEdge = false;
            }
        }
        else
        {
            playonce = false;
            enableCatchMovement = true;
            isHoldingEdge = false;
            animator.SetBool("Climb_Up", false);
        }
    }

    IEnumerator ClimbUp()
    {
        for (float time = 0; time < 0.3f; time += Time.deltaTime)
        {
            var climbY = Mathf.Lerp(catchCollider.transform.position.y, player.transform.position.y - 0.2f, time);
            catchCollider.transform.position = new Vector2(catchCollider.transform.position.x, climbY);
            if (time > 0.25f)
            {
                player.transform.position = new Vector2(player.transform.position.x + 0.2f * direction, player.transform.position.y);
                yield return null;
            }
            yield return null;
        }
        catchCollider.transform.localPosition = startPos;
        climbingInProgress = false;

        attackMovement.AttackMovementEnabled = true;
    }
}
