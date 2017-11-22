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
    private LayerMask playerLayer;

    [SerializeField]
    private float playerRange;

    [SerializeField]
    private float walkSpeed = 1f;

    private Animator animator;
    private PlayerController playerController;
    private EnterTerritory enterTerritory;
    private CameraFollow cameraFollow;

    private bool isLanding;
    private bool isStopping;
    private bool fireBlow1;
    private bool fireBlow2;
    private bool isWalking;
    private bool isIdle;
    private bool isReady;




    private Vector2 startPosition;
    private Vector2 result;

    private float expectedOrthographicsSize = 18f;
    private float startOrthographicSize;

    [HideInInspector]
    public float minCameraYPosition = -93f;

    private bool isFacingRight;
    private bool isFacingLeft = true;
    private bool playerInRange;

    private bool playCoroutineOnce;

    private float currentLerpTime = 0;
    private float lerpTime = 0.4f;
    private float Perc;
    private int direction = -1;

    private int flyStage = 0;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        playerController = FindObjectOfType<PlayerController>();
        enterTerritory = FindObjectOfType<EnterTerritory>();
        startOrthographicSize = mainCamera.orthographicSize;
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    protected void Start()
    {

    }

    protected void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (enterTerritory.bossEnabled)
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
                    Debug.Log("case4");
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
                    StartFireBlow();

                    yield return new WaitForSeconds(4f);
                    animator.SetBool("isIdle", true);
                    lerpTime = 2f;

                    mainCamera.transform.position = Vector3.Lerp(new Vector3(checkPoint[3].transform.position.x, minCameraYPosition, -10), 
                                                                 new Vector3(enterTerritory.playerAfterEnteringTerritoryPos.x, minCameraYPosition, -10), Perc);

                    currentLerpTime += Time.deltaTime;

                    break;
                }
        }
        
        

        if (isReady)
        {
            var callOnce = true;
            if (callOnce)
            {
                flyStage = -1;
                Invoke("BackToNormal", 1.5f);
                callOnce = false;
            }

            Walking();
            StartCoroutine(CheckPlayerPosition());
        }
    }

    private void StartFireBlow()
    {
        isWalking = true;
        FireBlow2();

        Invoke("StartWalk", 2f);
        return;
    }

    private void StartWalk()
    { 
        animator.SetBool("isIdle", true);
        isReady = true; // Start Walking coroutine
        isWalking = false; // Start walking 
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
        return;
    }

    IEnumerator FireBlow1()
    {
        yield return new WaitForSeconds(4.5f);
        animator.SetTrigger("fireBlow1");
        isWalking = true;
        yield return new WaitForSeconds(1f);
        isWalking = false;
        fireBlow1 = false;
        yield return 0;
    }
    
    private void FireBlow2()
    {
        animator.SetTrigger("fireBlow2");
        return;
    }

    private void Walking()
    {
        if (!isWalking)
        {
            transform.position = new Vector2(transform.position.x + (0.4f * Time.fixedDeltaTime * direction), transform.position.y);
        }

        if (!fireBlow1)
        {
            StartCoroutine(FireBlow1());
            fireBlow1 = true;
        }
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

    public void BackToNormal()
    {
        EnterTerritory.IsCharacterControlEnabled = false;
        cameraFollow.stopFollow = false;
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
