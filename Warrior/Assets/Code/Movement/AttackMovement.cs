using System.Collections;
using System.Collections.Generic;
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


    public bool attackRequest { get; set; }
    private bool currentState;
    private bool isAttackOnColldown;

    private float timer = 0;
    private float attackCooldown = 0.35f;
    private int attackCount = 0;
    private bool attackPressed;
    private bool nextComboStageEnabled;

    /// <summary>
    /// Waits for next frame in case to avoid checking if statement for same frame when button was pressed
    /// </summary>
    private bool waitForOneFrame;

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
        attackTrigger.enabled = false;
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Attack") && !isAttacking)
        {
            switch (attackCount)
            {
                case 0:
                    {
                        animator.SetBool("Attack", true);
                        StartCoroutine(ResetAnimationLogic("Attack"));
                        attackCount++;

                        StartCoroutine(CheckIfContinueComboHit(0f));
                        break;
                    }
                case 1:
                    {
                        animator.SetBool("Attack2", true);
                        StartCoroutine(ResetAnimationLogic("Attack2"));
                        attackCount++;

                        StartCoroutine(CheckIfContinueComboHit(0f));
                        break;
                    }
                case 2:
                    {
                        animator.SetBool("Attack3", true);
                        StartCoroutine(ResetAnimationLogic("Attack3"));
                        attackCount = 0;
                        break;
                    }
            }
            screenShake.shakeScreenOnAttack = true;
            audioManager.attackSound[Random.Range(0, 2)].Play();
            isAttacking = true;
            attackTrigger.enabled = true;
            currentState = turnAround.isFacingLeft;

            //if (isAttacking && (currentState == turnAround.isFacingLeft))
            {
                if (!isAttackOnColldown)
                {
                    StartCoroutine(AttackColldown());
                    isAttackOnColldown = true;
                }
            }
            //else
            {
                //isAttacking = false;
                //attackTrigger.enabled = false;
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
            Debug.Log("pressed in time");
            nextComboStageEnabled = false;
        }
    }

    IEnumerator ResetAnimationLogic(string parameter)
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(parameter, false);
    }
}
