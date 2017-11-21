using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterTerritory : MonoBehaviour
{
    //Attach Main Camera
    [SerializeField]
    public Camera mainCamera;

    [SerializeField]
    public float camSizingWaitTime = 2f;

    [SerializeField]
    public float camXPos = 21f;

    [SerializeField]
    public float camYPos = 4f;

    [SerializeField]
    public float camScale = 2f;

    [HideInInspector]
    public bool bossEnabled;

    [SerializeField]
    Transform dragon;

    [SerializeField]
    Transform ground;

    [SerializeField]
    Transform Dragon;

    DragonController dragonController;
    Collider2D myCollider;
    PlayerController playerController;
    CameraFollow cameraFollow;


    public Vector3 dragonStartPosition;
    public Vector3 startPosition;
    public Vector3 cameraStartPosition;
    private float endTime;
    private float percentage;
    private float minCameraY = -95f;

    private bool stopFollowVertical;

    public static bool IsCharacterControlEnabled;

    protected void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        playerController = FindObjectOfType<PlayerController>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        dragonController = FindObjectOfType<DragonController>();


        dragonStartPosition = new Vector3(dragon.transform.position.x, dragon.transform.position.y, -11);
    }

    protected void Start()
    {
        myCollider.isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossEnabled)
        {
            IsCharacterControlEnabled = true;
            cameraFollow.stopFollow = true;
            bossEnabled = true;
            cameraStartPosition = cameraFollow.transform.position;

            startPosition = playerController.transform.position;
        }
    }

    protected void Update()
    {

    }

}

