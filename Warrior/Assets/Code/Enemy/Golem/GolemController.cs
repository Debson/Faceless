using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : MonoBehaviour
{
    [SerializeField]
    private float attackFreq = 3f;

    [SerializeField]
    private float attackingRange;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    private GameObject deathCollider;

    AudioManager audioManager;
    Animator animator;
    HurtEnemyOnContact hurtEnemyOnContact;
    HurtPlayerOnContact hurtPlayerOnContact;
    ScreenShake screenShake;
    FloorDetector floorDetector;
    RotateEnemy rotateEnemy;
    EnemyHealthManager enemyHealthManager;
    Collider2D myCollider;
    HealthManager healthManager;

    private bool playerInRange;
    private bool _callOnce;
    private bool _death;

    protected void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        screenShake = FindObjectOfType<ScreenShake>();
        floorDetector = FindObjectOfType<FloorDetector>();
        hurtPlayerOnContact = FindObjectOfType<HurtPlayerOnContact>();
        rotateEnemy = GetComponent<RotateEnemy>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        myCollider = GetComponent<Collider2D>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    protected void LateUpdate()
    {
        SetAnimation();
        playerInRange = Physics2D.OverlapCircle(transform.position, attackingRange, playerLayer);

        if(playerInRange && !_callOnce)
        {
            StartCoroutine(Smash());
            _callOnce = true;
        }
        Debug.Log(transform.rotation.z);
    }

    IEnumerator Smash()
    {
        yield return new WaitForSeconds(attackFreq);
        if (playerInRange && !_death)
        {
            rotateEnemy.stopRotate = true;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.35f);
            screenShake.shakeScreen = true;

            if (floorDetector.isTouchingFloor)
            {
                HurtPlayerOnContact.onAttackDamage += healthManager.AttackPlayer;
            }
            yield return new WaitForSeconds(1f);
        }
            _callOnce = false;
            rotateEnemy.stopRotate = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackingRange);
    }

    private void SetAnimation()
    {
        if(hurtEnemyOnContact.isHurt)
        {

        }

        if(enemyHealthManager.GetHealth() <= 0 && !_death)
        {
            animator.SetTrigger("Dead");
            StopAllCoroutines();
            myCollider.enabled = false;
            deathCollider.active = true;
            rotateEnemy.enabled = false;
            _death = true;
            Destroy(gameObject, 6f);
        }

    }
}
