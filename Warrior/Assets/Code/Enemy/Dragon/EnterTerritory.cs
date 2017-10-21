using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterTerritory : MonoBehaviour
{
    //Attach Main Camera
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    public float camSizingWaitTime = 2f;

    [SerializeField]
    public float camXPos = 21f;

    [SerializeField]
    public float camYPos = 4f;

    [SerializeField]
    public float camScale = 2f;

    Collider2D myCollider;
    PlayerController playerController;
    
    private float endTime;
    private float percentage;

    private bool bossEnabled;

    public static bool IsCharacterControlEnabled;

    protected void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        playerController = GetComponent<PlayerController>();
    }

    protected void Start()
    {
        myCollider.isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !bossEnabled)
        {
            StartCoroutine(DragonEnter());
        }
    }
    
    IEnumerator DragonEnter()
    {
        bossEnabled = true;
        IsCharacterControlEnabled = true;
        mainCamera.GetComponent<CameraFollow>().enabled = false;
        //yield return new WaitForSeconds(2);
        endTime = 0;
        float camStartSize = mainCamera.orthographicSize;
        float camStartXPosition = mainCamera.transform.position.x;
        float camStartYPosition = mainCamera.transform.position.y;
        float camStartZPosition = mainCamera.transform.position.z;

        while (endTime < camSizingWaitTime)
        {
            percentage = endTime / camSizingWaitTime;
            endTime += Time.deltaTime;
            //Debug.Log(Mathf.Sqrt(percentage+1));
            yield return new WaitForFixedUpdate();

            mainCamera.orthographicSize = (percentage * camScale) + camStartSize;
            mainCamera.transform.position = new Vector3(camStartXPosition + (Mathf.Pow(percentage, 1.4f)  * camXPos), (percentage * camYPos) + camStartYPosition,
                                                        mainCamera.transform.position.z);
        }

        yield return new WaitForSeconds(3);
        
        // Boss enters
        // Then everything comes back to normal
        mainCamera.orthographicSize = camStartSize;
        mainCamera.transform.position = new Vector3(camStartXPosition, camStartYPosition, camStartZPosition);
        mainCamera.GetComponent<CameraFollow>().enabled = true;
        IsCharacterControlEnabled = false;

        yield return 0;
    }

}
