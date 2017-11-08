using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour
{

    WanderWalkController wanderWalkController;
    Animator animator;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        //Debug.Log(wanderWalkController.isRunning);
        animator.SetBool("isRunning", wanderWalkController.isRunning);
        animator.SetBool("isWalking", !wanderWalkController.playerInRange);

    }

}
