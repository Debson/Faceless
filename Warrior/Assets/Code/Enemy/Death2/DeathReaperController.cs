using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReaperController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] laser;

    [SerializeField]
    private Transform laserShootPoint;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    private float minLaserCastRange = 20f;

    [SerializeField]
    private float maxLaserCastRange = 10f;

    [SerializeField]
    public float laserSpeed = 10f;

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController playerController;
    LaserController laserController;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;

    public  GameObject[] laserArray { get; private set; }
    private Vector2 start;
    private Vector2 end;
    private Vector2 temp;

    public int direction { get; set; }
    private float time = 0f;

    private bool prepareToSlash;
    private bool slash;
    private bool castLaser;
    private bool playerInMinCastRange;
    private bool playerInMaxCastRange;

    private bool callOnce;
    private bool callOnce2;
    private bool callOnce3;

    private bool[] playOnce;

    protected void Awake()
    {
        laserArray = new GameObject[5];
        playOnce = new bool[5];
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        laserController = FindObjectOfType<LaserController>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        direction = -1;
    }

    protected void LateUpdate()
    {
        animator.SetBool("isRunning", wanderWalkController.isRunning);

        playerInMinCastRange = Physics2D.OverlapCircle(transform.position, minLaserCastRange, playerLayer);
        playerInMaxCastRange = Physics2D.OverlapCircle(transform.position, maxLaserCastRange, playerLayer);


        if (playerInMinCastRange)
        {
            if (wanderWalkController.playerInRange)
            {
                direction = wanderWalkController.isFacingLeft ? -1 : 1;

                if (!callOnce3)
                {
                    StartCoroutine(Laser());
                    callOnce3 = true;
                }
            }
        }
        else if(castLaser == false)
        {
            //callOnce3 = false;
            StartCoroutine(WaitBetweenLasers());
        }

        if (hurtPlayerOnContact.attackingAnimation)
        {
            StartCoroutine(Slash());
        }
        else
        {
            callOnce2 = false;
            callOnce = false;
        }

        SetSounds();
    }

    IEnumerator Slash()
    {
        if (!callOnce)
        {
            time = 0f;
            start = transform.position;
            end = playerController.transform.position;
            callOnce = true;
            animator.SetBool("prepareToSlash", true);
        }

        if (callOnce2)
        {
            Invoke("ReturnFromSlash", 0.5f);
        }
        else
        {
            while (time < 0.7f) // Go to 0.7 * distance to player
            {
                transform.position = Vector2.Lerp(start, end, time);
                time += Time.deltaTime;
                yield return 0;
            }
        }

        yield return new WaitForSeconds(0.15f);
        if(time >= 0.7f && !callOnce2)
        {
            animator.SetBool("prepareToSlash", false);
            time = 0f;
            end = start;
            start = transform.position;
            callOnce2 = true;
        }

    }

    IEnumerator Laser()
    {
        castLaser = true;
        animator.SetBool("castLaser", true);
        yield return new WaitForSeconds(0.1f); //Wait till enemy is facing player
        wanderWalkController.enabled = false;
        yield return new WaitForSeconds(1f); // delay before laser

        var rotation = laserShootPoint.rotation;
        var right = laserShootPoint.right;
        wanderWalkController.IgnorePlayerAboveEnemy = true;
        if (!playerInMaxCastRange)
        {
            for (int i = 4; i >= 0; i--)
            {
                var laserClone = Instantiate(laser[i], laserShootPoint.position, rotation);
                laserArray[i] = laserClone;
                laserClone.GetComponent<Rigidbody2D>().AddForce(right * laserSpeed * 90f);
                yield return new WaitForSeconds(laserSpeed / 150f);
            }
        }

        wanderWalkController.IgnorePlayerAboveEnemy = false;
        castLaser = false;
        animator.SetBool("castLaser", false);
        wanderWalkController.enabled = true;
    }

    IEnumerator WaitBetweenLasers()
    {
        callOnce3 = false;
        yield return new WaitForSeconds(2f);
    }

    private void SetSounds()
    {
        if(wanderWalkController.playerInRange && playOnce[1])
        {
            audioManager.reaperAgro[2].Play();
            playOnce[1] = false;
        }
        else if(!wanderWalkController.playerInRange)
        {
            playOnce[1] = true;
        }

        if (hurtEnemyOnContact.isHurt && playOnce[0])
        {
            audioManager.reaperHurt[Random.Range(2, 4)].Play();
            playOnce[0] = false;
        }
        else if (!hurtEnemyOnContact.isHurt)
        {
            playOnce[0] = true;
        }

        if(enemyHealthManager.GetHealth() <= 0 && playOnce[2])
        {
            audioManager.reaperDead[1].Play();
            playOnce[2] = false;
        }

        
        if (hurtPlayerOnContact.hit)
        {
            audioManager.reaperAttack[Random.Range(0, 2)].Play();
            hurtPlayerOnContact.hit = false;
        }
    }

    private void ReturnFromSlash()
    {
        while (time < 1f)
        {
            transform.position = Vector2.Lerp(start, end, time);
            time += Time.deltaTime;
            //yield return 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, minLaserCastRange);
        Gizmos.DrawWireSphere(transform.position, maxLaserCastRange);
    }
}
