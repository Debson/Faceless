using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public bool cameraBounds;

    [HideInInspector]
    public bool stopFollow;


    GameObject player;
    EnterTerritory enterTerritory;
    DragonController dragonController;

    private Vector2 velocity;
    private Vector3 minCameraPosition;
    private Vector3 maxCameraPosition;

    private float playerYPositionOnStart;
    private float playerXPositionOnStart;
    private float minYCameraPosition;
    private float cameraTimeX;
    private float cameraTimeY;

    protected void Awake()
    {
        dragonController = FindObjectOfType<DragonController>();
        player = GameObject.FindGameObjectWithTag("Player");
        enterTerritory = FindObjectOfType<EnterTerritory>();
    }

    protected void Start()
    {
        minYCameraPosition = -93f;
        playerYPositionOnStart = player.transform.position.y;
        playerXPositionOnStart = player.transform.position.x;
    }

    private void Update()
    {
        if (!dragonController.shakeScreen)
        {
            if (!stopFollow)
            {
                float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, cameraTimeX);
                float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, cameraTimeY);
                transform.position = new Vector3(posX, posY, transform.position.z);

                if (cameraBounds)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, playerXPositionOnStart + 9f, playerXPositionOnStart + 340f),
                                                     Mathf.Clamp(transform.position.y, playerYPositionOnStart + 4f, playerYPositionOnStart + 10f),
                                                     Mathf.Clamp(transform.position.z, -10, -10));
                }
            }
        }
    }
}
