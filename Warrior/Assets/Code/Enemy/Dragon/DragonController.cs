using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
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

    private bool isLanding;
    private bool isStopping;
    private bool fireBlow1;
    private bool fireBlow2;
    private bool isWalking;
    private bool isIdle;
    private bool isReady;




    private Vector2 startPosition;
    private Vector2 result;

    private bool isFacingRight;
    private bool isFacingLeft = true;
    private bool playerInRange;
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
    }

    protected void Start()
    {

    }

    protected void Update()
    {
        if (enterTerritory.bossEnabled)
        {
            StartFly();
        }
    }


    public void StartFly()
    {
        if (currentLerpTime >= lerpTime && flyStage != -1)
        {
            currentLerpTime = 0f;
            flyStage++;
        }

        Perc = currentLerpTime / lerpTime;

        switch (flyStage)
        {
            case 0: { lerpTime = 3.5f; enterTerritory.mainCamera.transform.position = Vector3.Lerp(enterTerritory.startPosition, enterTerritory.dragonStartPosition, Perc); break; }
            case 1: { lerpTime = 5f;  transform.position = Vector2.Lerp(enterTerritory.dragonStartPosition, checkPoint[0].transform.position, Perc); break; }
            case 2: { lerpTime = 3f; transform.position = Vector2.Lerp(checkPoint[0].transform.position, checkPoint[1].transform.position, Perc); break; }
            case 3: { lerpTime = 3f; transform.position = Vector2.Lerp(checkPoint[1].transform.position, checkPoint[2].transform.position, Perc); break; }
            case 4:
                {
                    animator.SetBool("isLanding", true);
                    lerpTime = 2f;
                    transform.position = Vector2.Lerp(checkPoint[2].transform.position, checkPoint[3].transform.position, Perc);
                    if (currentLerpTime > lerpTime * 0.7f)
                    {
                        animator.SetBool("isLanding", false);
                        animator.SetBool("isStopping", true);
                    }
                    break;
                }
            case 5:
                {
                    animator.SetBool("isStopping", false);
                    animator.SetBool("isIdle", true);
                    StartCoroutine(StartFireBlow());
                    flyStage = -1;
                    break;
                }
        }
        currentLerpTime += Time.deltaTime;

        if (isReady)
        {
            StartCoroutine(Walking());
            StartCoroutine(CheckPlayerPosition());
        }
    }

    // Two IEnumerators used only once on start
    IEnumerator StartFireBlow()
    {
        isWalking = true;
        animator.SetTrigger("fireBlow2");
        StartCoroutine(StartWalk());
        yield return 0;
    }

    IEnumerator StartWalk()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(3f);
        isReady = true;
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
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
    
    IEnumerator FireBlow2()
    {
        animator.SetTrigger("fireBlow2");
        yield return 0;
    }
    IEnumerator Walking()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (playerInRange && !isWalking)
        {
            transform.position = new Vector2(transform.position.x + (0.4f * Time.fixedDeltaTime * direction), transform.position.y);
            yield return 0;
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
        yield return 0;
    }


    IEnumerator RotateRight()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        StartCoroutine(FireBlow2());
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
        StartCoroutine(FireBlow2());
        yield return new WaitForSeconds(2f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);

        direction = 1;
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
