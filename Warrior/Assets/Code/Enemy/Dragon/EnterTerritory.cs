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
    FloorDetector floorDetector;


    public Vector3 dragonStartPosition;
    public Vector3 startPosition;
    public Vector3 playerAfterEnteringTerritoryPos;
    public Vector3 cameraStartPosition;


    private float endTime;
    private float percentage;
    private float minCameraY = -95f;
    private bool enteredTerritory;
    private bool stopFollowVertical;

    protected void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        playerController = FindObjectOfType<PlayerController>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        dragonController = FindObjectOfType<DragonController>();
        floorDetector = FindObjectOfType<FloorDetector>();


        //dragonStartPosition = new Vector3(dragon.transform.position.x, dragon.transform.position.y, -11);
    }

    protected void Start()
    {
        myCollider.isTrigger = true;
    }

    protected void Update()
    {
        if (enteredTerritory)
        {
            SavePlayerGroundedPosition();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossEnabled)
        {
            playerController.CharacterControlDisabled = true;
            cameraFollow.stopFollow = true;
            bossEnabled = true;
            enteredTerritory = true;
            cameraStartPosition = cameraFollow.transform.position;
            startPosition = playerController.transform.position;
        }
    }
    
    private void SavePlayerGroundedPosition()
    {
        if (floorDetector.isTouchingFloor)
        {
            playerAfterEnteringTerritoryPos = playerController.transform.position;
            enteredTerritory = false;
        }
    }
}

