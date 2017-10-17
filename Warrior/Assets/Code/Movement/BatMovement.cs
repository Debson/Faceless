using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BatMovement : MonoBehaviour
{
    private PlayerController playerController;

    public float moveSpeed;

    public float playerRange;

    public LayerMask playerLayer;

    public bool playerInRange;


    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    protected void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        if (playerInRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position,
                                                     moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
