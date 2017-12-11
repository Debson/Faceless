using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;

    WanderWalkController wanderWalkController;
    PlayerController playerController;

    BoxCollider2D playerCollider;
    private Vector3 difference;
    private float rotationZ;
    private bool playerInRange;
    private float playerBounds;

    protected void Awake()
    {
        wanderWalkController = FindObjectOfType<WanderWalkController>();
        playerController = FindObjectOfType<PlayerController>();
        playerCollider = playerController.GetComponent<BoxCollider2D>();
        playerBounds = playerController.GetComponent<Collider2D>().bounds.size.y;
    }

    protected void Update()
    {
        difference = new Vector3(playerController.transform.position.x, playerController.transform.position.y + 1.5f) - transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //Debug.DrawLine(transform.position, new Vector3(playerController.transform.position.x, playerController.transform.position.y + 1.5f), Color.red);

        playerInRange = Physics2D.OverlapCircle(transform.position, wanderWalkController.playerRange, playerLayer);
        if (playerInRange)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }
}
