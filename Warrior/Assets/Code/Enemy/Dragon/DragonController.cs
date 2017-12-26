using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [SerializeField]
    public Camera mainCamera;

    [SerializeField]
    private Transform[] checkPoint;

    [SerializeField]
    private float walkSpeed = 1f;

    [SerializeField]
    public float playerRange;

    [SerializeField]
    public LayerMask playerLayer;

    [SerializeField]
    private GameObject healthBar;


    private AudioManager audioManager;
    private Animator animator;
    private PlayerController playerController;
    private EnterTerritory enterTerritory;
    private CameraFollow cameraFollow;
    private Rigidbody2D myBody;
    private HurtEnemyOnContact hurtEnemyOnContact;
    private EnemyHealthManager enemyHealthManager;
    private Canvas canvas;
    private ScreenShake screenShake;

    public float minCameraYPosition { get; set; }

    private Vector2 startPosition;

    private float expectedOrthographicsSize;
    private float startOrthographicSize;

    /// <summary>
    /// Boolean to enter if statement only once
    /// </summary>
    private bool callOnce;
    /// <summary>
    /// When dragon is hit it reset attacking
    /// </summary>
    private bool access;
    private bool isWalking;
    private bool isReady;
    private bool attack;
    private bool playerInRange;
    private bool isFacingRight;
    private bool isFacingLeft = true;
    private bool isDead;


    private int direction;
    private int flyStage;

    private float currentLerpTime;
    private float lerpTime;
    private float Perc;
    private float dieDelay;

    /// <summary>
    /// Time by which camera should stay at dragon after landed
    /// </summary>
    public float Delay { get; private set; }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        playerController = FindObjectOfType<PlayerController>();
        enterTerritory = FindObjectOfType<EnterTerritory>();
        startOrthographicSize = mainCamera.orthographicSize;
        cameraFollow = FindObjectOfType<CameraFollow>();
        myBody = GetComponent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        canvas = healthBar.GetComponent<Canvas>();
        screenShake = FindObjectOfType<ScreenShake>();
    }

    protected void Start()
    {
        canvas.enabled = false;
        expectedOrthographicsSize = 18f;
        minCameraYPosition = -93f;
        currentLerpTime = 0;
        lerpTime = 0.4f;
        direction = -1;
        Delay = 0f;
        dieDelay = 0f;
        flyStage = 4;
    }

    protected void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (hurtEnemyOnContact.isHurt)
        {
            animator.SetTrigger("isHurt");
            audioManager.dragonPain[Random.Range(0, 4)].Play();
        }

        if (enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            StopAllCoroutines();
            animator.SetTrigger("exitAnimation");
            animator.SetTrigger("isDead");
            if(dieDelay > 0.1f)
            {
                audioManager.dragonStep.Play();
                isDead = true;
            }
            dieDelay += Time.deltaTime;
        }
        
        if (enterTerritory.bossEnabled && !isDead)
        {
            StartFly();
        }
    }

    private void StartFly()
    {
        if (currentLerpTime >= lerpTime && flyStage != -1)
        {
            currentLerpTime = 0f;
            flyStage++;
        }
        Perc = currentLerpTime / lerpTime;

        switch (flyStage)
        {
            case 0:
                {
                    Stage0();
                    break;
                }
            case 1:
                {
                    Stage1();
                    break;
                }
            case 2:
                {
                    Stage2();
                    break;
                }
            case 3:
                {
                    Stage3();
                    break;
                }
            case 4:
                {
                    Stage4();
                    break;
                }
            case 5:
                {
                    if (Delay >= 3f)
                    {
                        animator.SetBool("isIdle", true);
                        lerpTime = 2f;
                        mainCamera.transform.position = Vector3.Lerp(new Vector3(checkPoint[3].transform.position.x, minCameraYPosition, -10),
                                                                     new Vector3(enterTerritory.playerAfterEnteringTerritoryPos.x, minCameraYPosition, -10), Perc);
                        currentLerpTime += Time.deltaTime;
                        if (currentLerpTime >= 2f)
                        {
                            isReady = true;
                        }
                    }
                    else
                    {
                        animator.SetBool("isStopping", false);
                        if (!callOnce)
                        {
                            StartFireBlow();
                            callOnce = true;
                        }
                        Delay += Time.deltaTime;
                    }

                    break;
                }
        }

        if (isReady)
        {
            if (callOnce)
            {
                canvas.enabled = true;
                enemyHealthManager.healthBar.enabled = true;
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", true);
                flyStage = -1;
                Invoke("BackToNormal", 1.5f);
                callOnce = false;
                myBody.bodyType = RigidbodyType2D.Dynamic;
            }

            Walking();
            StartCoroutine(CheckPlayerPosition());
        }
        return;
    }

    IEnumerator CheckPlayerPosition()
    {
        yield return new WaitForSeconds(2f);

        if (playerController.transform.position.x > transform.position.x && !isFacingRight)
        {
            direction = 0;
            StartCoroutine(RotateLeft());
            isFacingRight = true;
        }
        else if (playerController.transform.position.x < transform.position.x && isFacingRight)
        {
            direction = 0;
            StartCoroutine(RotateRight());
            isFacingRight = false;
        }
    }

    IEnumerator RotateRight()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        FireBlow2();
        yield return new WaitForSeconds(2f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);

        direction = -1;
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    IEnumerator RotateLeft()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        FireBlow2();
        yield return new WaitForSeconds(2f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);

        direction = 1;
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    IEnumerator FireBlow1(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetBool("isWalking", false);
        isWalking = true;
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("fireBlow1");
        audioManager.dragonAttack1.Play();
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("isWalking", true);
        isWalking = false;
        attack = false;
    }

    IEnumerator Stomp(float delayBeforeStomp)
    {
        yield return new WaitForSeconds(delayBeforeStomp);

        isWalking = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        animator.SetTrigger("Roar");
        audioManager.dragonRoar.Play();
        yield return new WaitForSeconds(3.3f);
        Invoke("DragonJump", 0.4f);
        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(1f);
        audioManager.dragonStep.Play();

        screenShake.shakeScreen = true;
        isWalking = false;
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", true);
        attack = false;
    }

    private void Stage0()
    {
        audioManager.dragonFlapping.Play();
        cameraFollow.stopFollow = true;
        lerpTime = 3.5f;
        enterTerritory.mainCamera.transform.position = Vector3.Lerp(new Vector3(enterTerritory.startPosition.x, minCameraYPosition, -10), enterTerritory.dragonStartPosition, Perc);
        mainCamera.orthographicSize = Mathf.Lerp(startOrthographicSize, expectedOrthographicsSize, Perc);
        currentLerpTime += Time.deltaTime;
    }

    private void Stage1()
    {
        lerpTime = 2f;
        transform.position = Vector2.Lerp(enterTerritory.dragonStartPosition, checkPoint[0].transform.position, Perc);
        mainCamera.transform.position = Vector3.Lerp(enterTerritory.dragonStartPosition, checkPoint[0].transform.position, Perc);
        currentLerpTime += Time.deltaTime;
    }

    private void Stage2()
    {
        lerpTime = 3f;
        transform.position = Vector2.Lerp(checkPoint[0].transform.position, checkPoint[1].transform.position, Perc);
        mainCamera.transform.position = Vector3.Lerp(checkPoint[0].transform.position, checkPoint[1].transform.position, Perc);
        currentLerpTime += Time.deltaTime;
    }

    private void Stage3()
    {
        lerpTime = 3f;
        transform.position = Vector2.Lerp(checkPoint[1].transform.position, checkPoint[2].transform.position, Perc);
        mainCamera.transform.position = Vector3.Lerp(checkPoint[1].transform.position, checkPoint[2].transform.position, Perc);
        currentLerpTime += Time.deltaTime;
    }

    private void Stage4()
    {
        animator.SetBool("isLanding", true);
        lerpTime = 2f;
        transform.position = Vector2.Lerp(checkPoint[2].transform.position, checkPoint[3].transform.position, Perc);
        mainCamera.transform.position = Vector3.Lerp(checkPoint[2].transform.position, new Vector3(checkPoint[3].transform.position.x, minCameraYPosition, -10), Perc);
        mainCamera.orthographicSize = Mathf.Lerp(expectedOrthographicsSize, startOrthographicSize, Perc);

        if (currentLerpTime > lerpTime * 0.7f)
        {
            audioManager.dragonFlapping.Stop();
            animator.SetBool("isLanding", false);
            animator.SetBool("isStopping", true);
        }
        currentLerpTime += Time.deltaTime;
    }

    private void StartFireBlow()
    {
        isWalking = true;
        FireBlow2();
        animator.SetBool("isIdle", true);
        Invoke("StartWalk", 2f);
        return;
    }

    private void StartWalk()
    {
        isWalking = false; // enable if statement for walking
        animator.SetBool("isIdle", false);
        return;
    }

    private void FireBlow2()
    {
        audioManager.dragonAttack2.Play();
        animator.SetTrigger("fireBlow2");
        return;
    }

    private void Walking()
    {
        if (!isWalking)
        {
            transform.position = new Vector2(transform.position.x + (0.2f * Time.fixedDeltaTime * direction), transform.position.y);
        }

        if (!attack && playerInRange)
        {
            var random = Random.Range(0, 2);
            if (random == 1)
            {
                StartCoroutine(FireBlow1(4.5f));
            }
            else
            {
                StartCoroutine(Stomp(4f));
            }
            attack = true;
        }
        return;
    }

    private void DragonJump()
    {
        myBody.velocity = new Vector3(0, 4f, 0);
    }

    private void BackToNormal()
    {
        playerController.CharacterControlDisabled = false;
        cameraFollow.stopFollow = false;
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
