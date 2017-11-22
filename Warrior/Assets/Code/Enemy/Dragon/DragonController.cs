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

    private AudioManager audioManager;
    private Animator animator;
    private PlayerController playerController;
    private EnterTerritory enterTerritory;
    private CameraFollow cameraFollow;
    private Rigidbody2D myBody;
    private HurtEnemyOnContact hurtEnemyOnContact;
    private EnemyHealthManager enemyHealthManager;

    private bool isLanding;
    private bool isStopping;
    private bool fireBlow1;
    private bool fireBlow2;
    private bool isWalking;
    private bool isIdle;
    private bool isReady;
    private bool isDead;

    private bool attack;




    private Vector2 startPosition;
    private Vector2 result;

    private float expectedOrthographicsSize = 18f;
    private float startOrthographicSize;

    [HideInInspector]
    public float minCameraYPosition = -93f;

    [HideInInspector]
    public bool shakeScreen;

    private bool playerInRange;
    private bool isFacingRight;
    private bool isFacingLeft = true;
    private bool callOnce;

    private float currentLerpTime = 0;
    private float lerpTime = 0.4f;
    private float Perc;

    private int direction = -1;
    private int flyStage = 4;

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
    }

    protected void Start()
    {

    }

    protected void Update()
    {
        if(hurtEnemyOnContact.isHurt)
        {
            animator.SetTrigger("isHurt");
            audioManager.dragonPain[Random.Range(0, 4)].Play();
        }
        if(enemyHealthManager.GetHealth() <= 0)
        {
            animator.SetTrigger("isDead");
        }
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        //if (enterTerritory.bossEnabled)
        {
            StartCoroutine(StartFly());
        }
    }


    public IEnumerator StartFly()
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
                    audioManager.dragonFlapping.Play();
                    cameraFollow.stopFollow = true;
                    lerpTime = 3.5f;
                    enterTerritory.mainCamera.transform.position = Vector3.Lerp(new Vector3(enterTerritory.startPosition.x, minCameraYPosition, -10), enterTerritory.dragonStartPosition, Perc);
                    mainCamera.orthographicSize = Mathf.Lerp(startOrthographicSize, expectedOrthographicsSize, Perc);
                    currentLerpTime += Time.deltaTime;

                    break;
                }
            case 1:
                {
                    lerpTime = 2f;
                    transform.position = Vector2.Lerp(enterTerritory.dragonStartPosition, checkPoint[0].transform.position, Perc);
                    mainCamera.transform.position = Vector3.Lerp(enterTerritory.dragonStartPosition, checkPoint[0].transform.position, Perc);
                    currentLerpTime += Time.deltaTime;

                    break;
                }
            case 2:
                {
                    lerpTime = 3f;
                    transform.position = Vector2.Lerp(checkPoint[0].transform.position, checkPoint[1].transform.position, Perc);
                    mainCamera.transform.position = Vector3.Lerp(checkPoint[0].transform.position, checkPoint[1].transform.position, Perc);
                    currentLerpTime += Time.deltaTime;

                    break;
                }
            case 3:
                {
                    lerpTime = 3f;
                    transform.position = Vector2.Lerp(checkPoint[1].transform.position, checkPoint[2].transform.position, Perc);
                    mainCamera.transform.position = Vector3.Lerp(checkPoint[1].transform.position, checkPoint[2].transform.position, Perc);
                    currentLerpTime += Time.deltaTime;

                    break;
                }
            case 4:
                {
                    audioManager.dragonFlapping.Stop();
                    animator.SetBool("isLanding", true);
                    lerpTime = 2f;
                    transform.position = Vector2.Lerp(checkPoint[2].transform.position, checkPoint[3].transform.position, Perc);
                    mainCamera.transform.position = Vector3.Lerp(checkPoint[2].transform.position, new Vector3(checkPoint[3].transform.position.x, minCameraYPosition, -10), Perc);
                    mainCamera.orthographicSize = Mathf.Lerp(expectedOrthographicsSize, startOrthographicSize, Perc);

                    if (currentLerpTime > lerpTime * 0.7f)
                    {
                        animator.SetBool("isLanding", false);
                        animator.SetBool("isStopping", true);
                    }
                    currentLerpTime += Time.deltaTime;
                    break;
                }
            case 5:
                {
                    animator.SetBool("isStopping", false);
                    if (!callOnce)
                    {
                        StartCoroutine(StartFireBlow());
                        callOnce = true;
                    }
                    

                    yield return new WaitForSeconds(4f);
                    animator.SetBool("isIdle", true);
                    lerpTime = 2f;

                    mainCamera.transform.position = Vector3.Lerp(new Vector3(checkPoint[3].transform.position.x, minCameraYPosition, -10), 
                                                                 new Vector3(enterTerritory.playerAfterEnteringTerritoryPos.x, minCameraYPosition, -10), Perc);

                    currentLerpTime += Time.deltaTime;
                    if(currentLerpTime >= 2f)
                    {
                        isReady = true;
                    }
                    break;
                }
        }

        if (isReady)
        {
            if (callOnce)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", true);
                flyStage = -1;
                Invoke("BackToNormal", 0.1f);
                callOnce = false;
                myBody.bodyType = RigidbodyType2D.Dynamic;
            }
            Walking();
            StartCoroutine(CheckPlayerPosition());
        }
    }

    IEnumerator StartFireBlow()
    {
        isWalking = true;
        FireBlow2();
        yield return new WaitForSeconds(1f);
        animator.SetBool("isIdle", true);

        Invoke("StartWalk", 2f);
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

        if (!attack && playerInRange && !hurtEnemyOnContact.isHurt)
        {
            int random = Random.Range(0, 2);
            if (random == 1)
            {
                StartCoroutine(FireBlow1(4.5f));
            }
            else
            {
                StartCoroutine(Stomp(4f));
            }
            Debug.Log(random);
            attack = true;
        }
        return;
    }


    IEnumerator CheckPlayerPosition()
    {
        yield return new WaitForSeconds(2f);

        if(playerController.transform.position.x > transform.position.x && !isFacingRight)
        {
            direction = 0;
            StartCoroutine(RotateLeft());
            isFacingRight = true;
        }
        else if(playerController.transform.position.x < transform.position.x && isFacingRight)
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

        shakeScreen = true;
        isWalking = false;
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", true);
        attack = false;
    }

    private void DragonJump()
    {
        myBody.velocity = new Vector3(0, 4f, 0);
    }

    public void BackToNormal()
    {
        Debug.Log("ready");
        EnterTerritory.IsCharacterControlEnabled = false;
        cameraFollow.stopFollow = false;
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
