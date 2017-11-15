using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfController : MonoBehaviour
{

    [SerializeField]
    private float playerRange;

    [SerializeField]
    private float shootingSpeed = 1f;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    Transform shootPoint;

    [SerializeField]
    GameObject arrow;

    [SerializeField]
    Transform enemyHealthBar;

    [SerializeField]
    Transform bow;

    [SerializeField]
    GameObject disableOnDeath;

    [SerializeField]
    GameObject deathCollider;

    public float direction = -1;

    Animator animator;
    PlayerController playerController;
    ElfController elfController;
    RightArmMovement rightArmMovement;
    EnemyHealthManager enemyHealthManager;
    HurtEnemyOnContact hurtEnemyOnContact;


    private Vector3 difference;
    private float rotationZ;
    private float playerBounds;
    private bool playerInRange;
    private bool isFacingRight;
    private bool flipHealthBar;
    private bool shoot;


    protected void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        playerBounds = playerController.GetComponent<Collider2D>().bounds.size.y;
        rightArmMovement = GetComponent<RightArmMovement>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
    }

    protected void Start()
    {

    }

    protected void Update()
    {
        //animator.SetBool("isHurt", hurtEnemyOnContact.isHurt);

        StartCoroutine(OnDeath());
    }

    IEnumerator OnDeath()
    {
        if (enemyHealthManager.GetHealth() <= 0)
        {
            animator.SetBool("isDead", true);
            disableOnDeath.SetActive(false);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
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
    }


    IEnumerator ShootArrow()
    {
            StartCoroutine(rightArmMovement.MoveHandBeforeShoot());
            Instantiate(arrow, shootPoint.position, shootPoint.rotation, shootPoint.transform);
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
