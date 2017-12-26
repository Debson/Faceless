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
    HurtEnemyOnContact hurtEnemyOnContact;
    WalkMovement walkMovement;

    public delegate void OnAttackShake();
    public static event OnAttackShake onFirstAttack;
    public static event OnAttackShake onSecondAttack;
    public static event OnAttackShake onThirdAttack;

    public bool AttackMovementEnabled { get; set; }
    public int attackCount = 0;

    private bool currentState;
    private bool isAttackOnColldown;

    private float timer = 0;
    private float attackCooldown = 0.35f;
    private float firstAttackCooldown = 0.5f;
    private bool attackPressed;
    private bool nextComboStageEnabled;

    /// <summary>
    /// Waits for next frame in case to avoid checking if statement for same frame when button was pressed
    /// </summary>
    private bool waitForOneFrame;
    private bool firstAttackInJump;
    private bool startCombo;
    /// <summary>
    /// Prevent player from attacking after he got hurted
    /// </summary>
    private bool stopAttack;

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
        hurtEnemyOnContact = FindObjectOfType<HurtEnemyOnContact>();
        walkMovement = GetComponent<WalkMovement>();
        AttackMovementEnabled = true;
        attackTrigger.enabled = false;
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Attack") && !isAttacking && AttackMovementEnabled && !stopAttack)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !floorDetector.isTouchingFloor)
            {
                CheckComboStage("AttackInAir+Up");
                audioManager.comboSound[3].Play();

            }
            else if(Input.GetKey(KeyCode.UpArrow))
            {
                CheckComboStage("Attack+Up");
                audioManager.comboSound[4].Play();
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
                    if (startCombo)
                    {
                        CheckComboStage("Attack", "Attack2", "Attack3");
                    }
                    else
                    {
                        attackCount = 0;
                        audioManager.comboSound[0].Play();
                        audioManager.attackSound[Random.Range(0, 2)].Play();
                        CheckComboStage("Attack");
                    }
                }
                else
                {
                    firstAttackInJump = true;
                    if (startCombo)
                    {
                        CheckComboStage("Attack_In_Air", "Attack_In_Air2", "Attack_In_Air3");
                    }
                    else
                    {
                        attackCount = 0;
                        audioManager.comboSound[0].Play();
                        audioManager.attackSound[Random.Range(0, 1)].Play();
                        CheckComboStage("Attack_In_Air");
                    }
                }
            }

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
        else if(stopAttack)
        {
            StartCoroutine(StopAttackDelay());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "EnemyAttackTrigger" || collision.tag == "Enemy")
        {
            startCombo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        startCombo = false;
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
                    audioManager.comboSound[0].Play();

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
                    audioManager.comboSound[1].Play();
                    StartCoroutine(CheckIfContinueComboHit(0f));
                    break;
                }
            case 2:
                {
                    animator.SetBool(animation3, true);
                    StartCoroutine(ResetAnimationLogic(animation3));
                    attackCount = 0;

                    onThirdAttack += screenShake.ShakeOnThirdAttack;
                    audioManager.comboSound[2].Play();

                    if (onThirdAttack != null)
                    {
                        onThirdAttack();
                    }

                    break;
                }
        }
    }

    public void SetStopAttack(bool value)
    {
        stopAttack = value;
    }

    IEnumerator StopAttackDelay()
    {
        yield return new WaitForSeconds(walkMovement.knockBackLength);
        stopAttack = false;
    }

    IEnumerator AttackColldown()
    {
        var time = attackCooldown;
        switch(attackCount)
        {
            case 1:
                {
                    time = firstAttackCooldown;
                    break;
                }
            default:
                {
                    time = attackCooldown;
                    break;
                }
        }
        yield return new WaitForSeconds(time);
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
