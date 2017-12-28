using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfController : MonoBehaviour
{
    [SerializeField] private float playerRange;

    [SerializeField] private float shootingSpeed = 1f;

    [SerializeField] private float arrowSpeed;

    [SerializeField] private  GameObject arrow;

    [SerializeField] private GameObject body;

    [SerializeField] private Transform arrowShootPoint;

    [SerializeField] private Transform enemyHealthBar;

    [SerializeField] private Transform bow;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Collider2D deathCollider;

    Animator animator;
    PlayerController playerController;
    ElfController elfController;
    RightArmMovement rightArmMovement;
    EnemyHealthManager enemyHealthManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    AudioManager audioManager;
    EnemyHealthBar healthBar;

    public float direction { get; set; }

    private Vector3 difference;

    private float rotationZ;
    private float playerBounds;

    private bool playerInRange;
    private bool isFacingRight;
    private bool flipHealthBar;
    private bool shootArrow;
    private bool isDead;
    private bool playOnce;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        rightArmMovement = GetComponent<RightArmMovement>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        audioManager = FindObjectOfType<AudioManager>();
        healthBar = GetComponent<EnemyHealthBar>();
        playerBounds = playerController.GetComponent<Collider2D>().bounds.size.y;
        deathCollider.enabled = false;
    }

    protected void Start()
    {
        direction = -1;
    }

    protected void LateUpdate()
    {
        if (enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            OnDeath();
        }
        else if (!isDead)
        {
            SetAnimationLogic();
            playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
            if (playerInRange)
            {
                StartCoroutine(RotateElfAndShoot());
            }
        }
    }

    private void OnDeath()
    {
        body.SetActive(false);
        animator.SetTrigger("isDead");
        audioManager.elfDeath[0].Play();
        gameObject.GetComponent<Collider2D>().enabled = false;
        deathCollider.enabled = true;
        rightArmMovement.enabled = false;
        healthBar.enabled = false;
        hurtEnemyOnContact.enabled = false;
        isDead = true;
        Destroy(gameObject, 6f);
    }

    private void SetAnimationLogic()
    {
        if (hurtEnemyOnContact.isHurt && !playOnce)
        {
            animator.SetTrigger("isHurt");
            audioManager.elfHurt[Random.Range(0, 2)].Play();
            playOnce = true;
        }
        else if(!hurtEnemyOnContact.isHurt)
        {
            playOnce = false;
        }
    }

    IEnumerator RotateElfAndShoot()
    {
        if (transform.position.x < playerController.transform.position.x && !isFacingRight)
        {
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            enemyHealthBar.transform.rotation *= Quaternion.Euler(0, 180, 0);

            isFacingRight = true;
            direction = 1;
            yield return new WaitForSeconds(0.2f);
        }
        else if (transform.position.x > playerController.transform.position.x && isFacingRight)
        {
            transform.rotation *= Quaternion.Euler(0, -180, 0);
            enemyHealthBar.transform.rotation *= Quaternion.Euler(0, 180, 0);

            isFacingRight = false;
            direction = -1;
            yield return new WaitForSeconds(0.2f);
        }

        if (rightArmMovement.startStretch)
        {
            StartCoroutine(ShootArrow());
            rightArmMovement.startStretch = false;
        }

        StartCoroutine(BowFollowPlayer());
    }

    IEnumerator ShootArrow()
    {
        var rotation = arrowShootPoint.rotation;
        var right = arrowShootPoint.right;
        StartCoroutine(rightArmMovement.MoveHandBeforeShoot());
        var arrowClone = Instantiate(arrow, arrowShootPoint.position, rotation, transform);
        arrowClone.GetComponent<Rigidbody2D>().AddForce(right * arrowSpeed * 90f);
        Destroy(arrowClone, 4f);

        audioManager.bowArrow.Play();
        yield return new WaitForSeconds(shootingSpeed);
        rightArmMovement.startStretch = true;
    }

    IEnumerator BowFollowPlayer()
    {
        difference = playerController.transform.position - bow.transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //Debug.DrawLine(bow.transform.position, playerController.transform.position, Color.red);

        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        if (playerInRange)
        {
            bow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
        yield return new WaitForSeconds(0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
