using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReaperController : MonoBehaviour
{

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController playerController;

    private Vector2 start;
    private Vector2 end;
    private Vector2 temp;

    private int d = 0;
    private float time = 0f;

    private bool prepareToSlash;
    private bool slash;
    private bool onlyOnce;
    private bool callOnce;
    private int slashAnimationCount = 0;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
    }

    protected void Update()
    {
        //animator.SetBool("isRunning", wanderWalkController.isRunning);
        if (hurtPlayerOnContact.attackingAnimation)
        {
            StartCoroutine(Slash());
        }
        else
        {
            callOnce = false;
            onlyOnce = false;
        }
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("death2_slash"));
    }

    IEnumerator Slash()
    {
        if (!onlyOnce)
        {
            time = 0f;
            start = transform.position;
            end = playerController.transform.position;
            onlyOnce = true;
            animator.SetBool("prepareToSlash", true);
        }

        if (callOnce)
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
        if(time >= 0.7f && !callOnce)
        {
            animator.SetBool("prepareToSlash", false);
            time = 0f;
            end = start;
            start = transform.position;
            callOnce = true;
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
}
