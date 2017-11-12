using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private float shootSpeed;

    [SerializeField]
    Transform shootPoint;

    PlayerController playerController;

    private Vector2 result;
    private Vector2 extendedPosition;
    private Vector2 playerPositionOnStart;
    private Vector2 arrowPositionOnStart;
    private Quaternion arrowZ;

    private float currentLerpTime = 0;
    private float lerpTime = 2f;

    protected void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerPositionOnStart = FindObjectOfType<PlayerController>().transform.position;
        arrowPositionOnStart = transform.position;

        extendedPosition = playerPositionOnStart - arrowPositionOnStart;
        extendedPosition = extendedPosition * 1.5f;
        result = playerPositionOnStart + extendedPosition;

        arrowZ = transform.rotation;
    }

    protected void Update()
    {
        if(currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        transform.rotation = arrowZ;
        float Perc = currentLerpTime / lerpTime;

        //Debug.DrawRay(arrowPositionOnStart, extendedPosition);
        transform.position = Vector2.Lerp(arrowPositionOnStart, result, Perc);

        currentLerpTime += Time.deltaTime;

        Destroy(gameObject, 3f);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("hit");
            //Destroy(gameObject);
        }
    }
}
