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

    private bool playerInRange;
    private float sinus;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        elfController = GetComponentInParent<ElfController>();
    }

    protected void Update()
    {
        //Debug.Log(playerController.transform.position.y);
        //Debug.Log(elfController.transform.position.y);
        //Debug.Log(Vector2.Distance(elfController.transform.position, playerController.transform.position));

        sinus = Mathf.Abs(elfController.transform.position.y - playerController.transform.position.y) / Vector2.Distance(elfController.transform.position, playerController.transform.position);
        //Debug.Log(sinus);



        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        Debug.Log(elfController.direction);
        if (playerInRange)
        {
            if (elfController.direction == -1)
            {
                transform.rotation = Quaternion.Euler(0, -180, Mathf.Rad2Deg * sinus * elfController.direction);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * sinus);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
