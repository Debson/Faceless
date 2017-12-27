using System.Collections;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    [SerializeField]
    private Collider2D attackTrigger;

    [SerializeField]
    private float delayToDeactivateCombo = 2f;

    [SerializeField]
    private Transform raycastPoint;

    AudioManager audioManager;
    TurnAround turnAround;
    ScreenShake screenShake;
    Animator animator;
    FloorDetector floorDetector;
    HurtEnemyOnContact hurtEnemyOnContact;
    WalkMovement walkMovement;
    PlayerController player;

    public delegate void OnAttackShake();
    public static event OnAttackShake onFirstAttack;
    public static event OnAttackShake onSecondAttack;
    public static event OnAttackShake onThirdAttack;

    public bool isAttacking { get; private set; }
    public bool AttackMovementEnabled { get; set; }
    public int attackCount = 0;

    private float timeLeft;
    private float attackCooldown = 0.45f;
    private float firstAttackCooldown = 0.5f;

    private float specialAttackCooldown = 2f;
    private bool attackPressed;
    private bool nextComboStageEnabled;

    /// <summary>
    /// Waits for next frame in case to avoid checking if statement for same frame when button was pressed
    /// </summary>
    private bool waitForOneFrame;
    private bool currentState;
    private bool isAttackOnColldown;
    private bool firstAttackInJump;
    private bool startCombo;
    private bool specialAttackActivated;
    /// <summary>
    /// Prevent player from attacking after he got hurted
    /// </summary>
    private bool stopAttack;

    protected void Awake()
    {
        turnAround = GetComponent<TurnAround>();
        audioManager = FindObjectOfType<AudioManager>();
        screenShake = FindObjectOfType<ScreenShake>();
        animator = GetComponent<Animator>();
        floorDetector = GetComponent<FloorDetector>();
        hurtEnemyOnContact = FindObjectOfType<HurtEnemyOnContact>();
        walkMovement = GetComponent<WalkMovement>();
        player = GetComponent<PlayerController>();
        AttackMovementEnabled = true;
        attackTrigger.enabled = false;
    }

    protected void Start()
    {
        timeLeft = specialAttackCooldown;
    }

    protected void Update()
    {
        if(startCombo && 
            (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")))
        {
            player.CharacterControlDisabled = true;
        }
        else if(startCombo)
        {
            //player.CharacterControlDisabled = false;
        }

        if (Input.GetButtonDown("Attack") && !isAttacking && AttackMovementEnabled && !stopAttack)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !floorDetector.isTouchingFloor)
            {
                SpecialAttackFromAir();
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                SpecialAttackFromGround();
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

                currentState = turnAround.isFacingLeft;
                StartAttackColldown();
            }
            // Animation have to be played in same direction as on start

        }
        else if (stopAttack)
        {
            //Cant attack when knockback
            StartCoroutine(StopAttackDelay());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            startCombo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        startCombo = false;
    }

    private void StartAttackColldown()
    {
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
            //player.CharacterControlDisabled = false;
        }
    }

    private void SpecialAttackFromGround()
    {
        if (!specialAttackActivated)
        {
            attackCount = 4;
            specialAttackActivated = true;
            CheckComboStage("Attack+Up");
            audioManager.comboSound[4].Play();

            StartCoroutine(SpecialAttackCooldown(0.1f));
        }
    }

    private void SpecialAttackFromAir()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall") && !specialAttackActivated)
        {
            attackCount = 5;
            specialAttackActivated = true;
            player.CharacterControlDisabled = true;
            CheckComboStage("AttackInAir+Up");
            audioManager.comboSound[3].Play();

            StartCoroutine(SpecialAttackCooldown(0.8f));
        }
    }

    public void CheckComboStage(string animation)
    {
        isAttacking = true;
        attackTrigger.enabled = true;

        animator.SetBool(animation, true);
        StartCoroutine(ResetAnimationLogic(animation));
    }

    private void CheckComboStage(string animation1, string animation2, string animation3)
    {
        isAttacking = true;
        attackTrigger.enabled = true;

        switch (attackCount)
        {
            case 0:
                {
                    animator.SetBool(animation1, true);
                    StartCoroutine(ResetAnimationLogic(animation1));
                    attackCount = 0;
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
                    if (onSecondAttack != null)
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
                    attackCount++;

                    onThirdAttack += screenShake.ShakeOnThirdAttack;
                    if (onThirdAttack != null)
                    {
                        onThirdAttack();
                    }
                    audioManager.comboSound[2].Play();
                    break;
                }
        }
    }

    public void SetStopAttack(bool value)
    {
        stopAttack = value;
    }

    public float GetSpecialAttackCooldown()
    {
        return specialAttackCooldown;
    }

    public float GetSpecialAttackCooldownLeft()
    {
        return timeLeft;
    }

    public void ShortenSpecialAttackTimeLeft(float time)
    {
        timeLeft += time;
    }

    IEnumerator SpecialAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        attackTrigger.enabled = false;
        player.CharacterControlDisabled = false;
        attackCount = 0;

        for (timeLeft = 0f; timeLeft < specialAttackCooldown; timeLeft += Time.deltaTime)
        {
            yield return null;
        }
        specialAttackActivated = false;
    }

    IEnumerator StopAttackDelay()
    {
        yield return new WaitForSeconds(walkMovement.knockBackLength);
        stopAttack = false;
    }

    IEnumerator AttackColldown()
    {
        var time = attackCooldown;
        switch (attackCount)
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
        if(attackCount > 2)
        {//reset attackCount after 3rd attack
            attackCount = 0;
        }
        player.CharacterControlDisabled = false;
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
        yield return new WaitForSeconds(attackCooldown - 0.25f);
        animator.SetBool(parameter, false);
    }

    IEnumerable ResetAnimationLogic(string parameter, float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(parameter, false);
    }
}
