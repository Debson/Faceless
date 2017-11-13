using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMovement : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    private float playerRange;


    PlayerController playerController;
    ElfController elfController;


    private Vector3 difference;
    private float rotationZ;
    private bool playerInRange;
    private float playerBounds;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerBounds = playerController.GetComponent<Collider2D>().bounds.size.y;
    }

    protected void Update()
    {
        difference = playerController.transform.position - transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Debug.DrawLine(transform.position, playerController.transform.position, Color.red);

        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        if (playerInRange)
        {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
