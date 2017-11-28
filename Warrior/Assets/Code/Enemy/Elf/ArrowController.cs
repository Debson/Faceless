using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private float shootSpeed;

    [SerializeField]
    Transform shootPoint;

    [SerializeField]
    private int minDamageToGive;

    [SerializeField]
    private int maxDamageToGive;

    PlayerController playerController;
    WalkMovement walkMovement;
    AudioManager audioManager;

    private Vector2 result;
    private Vector2 extendedPosition;
    private Vector2 playerPositionOnStart;
    private Vector2 arrowPositionOnStart;
    private Quaternion arrowZ;

    private float distance;
    private float multiplier;
    private const float constDist = 15f;
    private float currentLerpTime;
    private float lerpTime;
    private float speed;

    protected void Awake()
    {
        walkMovement = FindObjectOfType<WalkMovement>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    protected void Start()
    {
        currentLerpTime = 0f;
        playerController = FindObjectOfType<PlayerController>();
        playerPositionOnStart = FindObjectOfType<PlayerController>().transform.position;
        arrowPositionOnStart = transform.position;
        // IF DIST == 15, multiplier = 1
        distance = Vector2.Distance(arrowPositionOnStart, playerPositionOnStart);
        multiplier = distance / constDist;

        if(multiplier < 1)
        {
            multiplier = 20 / multiplier;
        }

        extendedPosition = playerPositionOnStart - arrowPositionOnStart;
        extendedPosition = extendedPosition * 2f * multiplier;
        lerpTime = 1.5f * multiplier * distance / constDist;

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

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
            audioManager.playerHurt[Random.Range(0, 2)].Play();
            walkMovement.Knockback(gameObject);
            Destroy(gameObject);
        }
    }
}
