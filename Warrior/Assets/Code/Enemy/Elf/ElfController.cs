using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfController : MonoBehaviour
{

    [SerializeField]
    public float playerRange;

    [SerializeField]
    Transform shootPoint;

    [SerializeField]
    GameObject arrow;


    PlayerController playerController;

    public float direction = -1;

    private bool playerInRange;
    private bool isFacingRight;
    private bool shoot;
 


    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    protected void Update()
    {
        if(transform.position.x < playerController.transform.position.x && !isFacingRight)
        {
           transform.rotation *= Quaternion.Euler(0, 180, 0);
            isFacingRight = true;
            direction = 1;
        }
        else if(transform.position.x > playerController.transform.position.x && isFacingRight)
        {
            transform.rotation *= Quaternion.Euler(0, -180, 0);
            isFacingRight = false;
            direction = -1;
        }

        if (!shoot)
        {
            StartCoroutine(ShootArrow());
            shoot = true;
        }

    }

    IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(arrow, shootPoint.position, shootPoint.rotation, shootPoint.transform);
        yield return new WaitForSeconds(1f);
        shoot = false;

    }

}
