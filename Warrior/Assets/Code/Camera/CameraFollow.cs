using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public bool cameraBounds;

    public delegate void OnFall();
    public static event OnFall onPlayerFall;

    PlayerController player;
    EnterTerritory enterTerritory;
    DragonController dragonController;
    ScreenShake screenShake;

    private Vector2 velocity;
    private Vector3 minCameraPosition;
    private Vector3 maxCameraPosition;

    public bool stopFollow { set; get; }

    private float playerYPositionOnStart;
    private float playerXPositionOnStart;
    private float minYCameraPosition;
    private float cameraTimeX;
    private float cameraTimeY;

    protected void Awake()
    {
        dragonController = FindObjectOfType<DragonController>();
        player = FindObjectOfType<PlayerController>();
        enterTerritory = FindObjectOfType<EnterTerritory>();
        screenShake = GetComponentInChildren<ScreenShake>();
    }

    protected void Start()
    {
        minYCameraPosition = -93f;
        playerYPositionOnStart = player.transform.position.y;
        playerXPositionOnStart = player.transform.position.x;
    }

    private void Update()
    {
        if(onPlayerFall != null)
        {
            onPlayerFall();
        }

        if (player == null)
        {
            return;
        }
        else
        {
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

    public void PlayerFall()
    {
        this.enabled = false;
        onPlayerFall -= PlayerFall;
    }
}
