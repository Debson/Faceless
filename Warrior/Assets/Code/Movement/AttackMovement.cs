using System.Collections;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    [SerializeField]
    private Collider2D attackTrigger;

    [SerializeField]
    private float delayToDeactivateCombo = 2f;

    AudioManager audioManager;
    TurnAround turnAround;
    ScreenShake screenShake;
    Animator animator;
    FloorDetector floorDetector;

    public delegate void OnAttackShake();
    public static event OnAttackShake onFirstAttack;
    public static event OnAttackShake onSecondAttack;
    public static event OnAttackShake onThirdAttack;

    public bool attackRequest { get; set; }
    private bool currentState;
    private bool isAttackOnColldown;

    private float timer = 0;
    private float attackCooldown = 0.33f;
    private int attackCount = 0;
    private bool attackPressed;
    private bool nextComboStageEnabled;

    /// <summary>
    /// Waits for next frame in case to avoid checking if statement for same frame when button was pressed
    /// </summary>
    private bool waitForOneFrame;
    private bool firstAttackInJump;

    public bool isAttacking
    {
        get; private set;
    }

    protected void Awake()
    {
        turnAround = GetComponent<TurnAround>();
        audioManager = FindObjectOfType<AudioManager>();
        screenShake = FindObjectOfType<ScreenShake>();
        animator = GetComponent<Animator>();
        floorDetector = GetComponent<FloorDetector>();

        attackTrigger.enabled = false;
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Attack") && !isAttacking)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !floorDetector.isTouchingFloor)
            {
                CheckComboStage("AttackInAir+Up");
            }
            else if(Input.GetKey(KeyCode.UpArrow))
            {
                CheckComboStage("Attack+Up");
            }
            else
            {
                if (floorDetector.isTouchingFloor)
                {
                    if (!firstAttackInJump)
                    {
                        attackCount = 0;
                        firstAttackInJump = true;
                    }
                    CheckComboStage("Attack", "Attack2", "Attack3");
                }
                else
                {
                    firstAttackInJump = true;
                    CheckComboStage("Attack_In_Air", "Attack_In_Air2", "Attack_In_Air3");
                }
            }
            audioManager.attackSound[Random.Range(0, 2)].Play();
            isAttacking = true;
            attackTrigger.enabled = true;
            currentState = turnAround.isFacingLeft;

            if (isAttacking && (currentState == turnAround.isFacingLeft))
            {
                if (!isAttackOnColldown)
                {
                    StartCoroutine(AttackColldown());
                    isAttackOnColldown = true;
                }
            }
            else
            {
                isAttacking = false;
                attackTrigger.enabled = false;
            }
        }
    }

    public void CheckComboStage(string animation)
    {
        animator.SetBool(animation, true);
        StartCoroutine(ResetAnimationLogic(animation));
    }

    private void CheckComboStage(string animation1, string animation2, string animation3)
    {
        switch (attackCount)
        {
            case 0:
                {
                    animator.SetBool(animation1, true);
                    StartCoroutine(ResetAnimationLogic(animation1));
                    attackCount++;

                    onFirstAttack += screenShake.ShakeOnFirstAtack;
                    if (onFirstAttack != null)
                    {
                        onFirstAttack();
                    }


                    StartCoroutine(CheckIfContinueComboHit(0f));
                    break;
                }
            case 1:
                {
                    animator.SetBool(animation2, true);
                    StartCoroutine(ResetAnimationLogic(animation2));
                    attackCount++;

                    onSecondAttack += screenShake.ShakeOnSecondAttack;
                    if(onSecondAttack != null)
                    {
                        onSecondAttack();
                    }

                    StartCoroutine(CheckIfContinueComboHit(0f));
                    break;
                }
            case 2:
                {
                    animator.SetBool(animation3, true);
                    StartCoroutine(ResetAnimationLogic(animation3));
                    attackCount = 0;

                    onThirdAttack += screenShake.ShakeOnThirdAttack;
                    if(onThirdAttack != null)
                    {
                        onThirdAttack();
                    }

                    break;
                }
        }
    }

    IEnumerator AttackColldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnColldown = false;
        isAttacking = false;
        attackTrigger.enabled = false;
    }

    IEnumerator CheckIfContinueComboHit(float timer)
    {
        while (timer < delayToDeactivateCombo && !nextComboStageEnabled)
        {
            timer += Time.deltaTime;

            if (!waitForOneFrame)
            {
                waitForOneFrame = true;
                yield return null;
            }
            if (Input.GetButtonDown("Attack"))
            {
                nextComboStageEnabled = true;
                waitForOneFrame = false;
            }

            yield return null;
        }

        if (timer > delayToDeactivateCombo - 0.1f)
        {
            attackCount = 0;
            waitForOneFrame = false;
        }
        else
        {
            nextComboStageEnabled = false;
        }
    }

    IEnumerator ResetAnimationLogic(string parameter)
    {
        yield return new WaitForSeconds(attackCooldown - 0.05f);
        animator.SetBool(parameter, false);
    }
}
