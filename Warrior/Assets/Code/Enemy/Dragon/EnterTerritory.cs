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
    private float endTime;
    private float percentage;
    private float minCameraY = -95f;
    private float dragonStartYPosition;
    private float dragonStartXPosition;
    private float startYPosition;
    private float startXPosition;

    private bool stopFollowVertical;

    public static bool IsCharacterControlEnabled;

    protected void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        playerController = FindObjectOfType<PlayerController>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        dragonController = FindObjectOfType<DragonController>();

        dragonStartXPosition = dragon.transform.position.x;
        dragonStartYPosition = dragon.transform.position.y;

        dragonStartPosition = dragon.transform.position;
    }

    protected void Start()
    {
        myCollider.isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossEnabled)
        {
            cameraFollow.follow = false;
            bossEnabled = true;

            startPosition = playerController.transform.position;
        }
    }

    protected void Update()
    {
        Debug.Log(ground.transform.position.y);
        //Debug.Log(mainCamera.transform.position.y);

        if (mainCamera.transform.position.y > minCameraY && stopFollowVertical && bossEnabled)
        {
            mainCamera.transform.position = new Vector3(dragon.transform.position.x, dragon.transform.position.y, -10);
        }
        else if (mainCamera.transform.position.y <= minCameraY && stopFollowVertical && bossEnabled)
        {
            stopFollowVertical = true;
        }

        if(stopFollowVertical)
        {
            mainCamera.transform.position = new Vector3(dragon.transform.position.x, minCameraY, -10);
        }
    }


    IEnumerator MoveCamera()
    {
        mainCamera.transform.position = Vector3.LerpUnclamped(startPosition, dragonStartPosition, Time.deltaTime * 0.1f);
        yield return new WaitForSeconds(3f);
        dragonController.StartFly();
    }

    IEnumerator DragonEnter()
    {
        IsCharacterControlEnabled = true;
        mainCamera.GetComponent<CameraFollow>().enabled = false;
        //yield return new WaitForSeconds(2);
        float camStartSize = mainCamera.orthographicSize;
        float camStartXPosition = mainCamera.transform.position.x;
        float camStartYPosition = mainCamera.transform.position.y;
        float camStartZPosition = mainCamera.transform.position.z;
        //mainCamera.transform.position = new Vector3(dragonController.transform.position.x, dragonController.transform.position.y, dragonController.transform.position.z);

    



        // Boss enters
        // Then everything comes back to normal
        mainCamera.orthographicSize = camStartSize;
        mainCamera.transform.position = new Vector3(camStartXPosition, camStartYPosition, camStartZPosition);
        mainCamera.GetComponent<CameraFollow>().enabled = true;
        IsCharacterControlEnabled = false;

        yield return 0;
    }

}


/*  while (endTime < camSizingWaitTime)
       {
           percentage = endTime / camSizingWaitTime;
           endTime += Time.deltaTime;
           //Debug.Log(Mathf.Sqrt(percentage+1));
           yield return new WaitForFixedUpdate();

           mainCamera.orthographicSize = (percentage * camScale) + camStartSize;
           mainCamera.transform.position = new Vector3(camStartXPosition + (Mathf.Pow(percentage, 1.4f)  * camXPos), (percentage * camYPos) + camStartYPosition,
                                                       mainCamera.transform.position.z);
       }

       yield return new WaitForSeconds(3);*/
