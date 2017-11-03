using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector2 velocity;

    public float cameraTimeX;
    public float cameraTimeY;

    public bool cameraBounds;

    public Vector3 minCameraPosition;
    public Vector3 maxCameraPosition;

    private float playerYPositionOnStart;
    private float playerXPositionOnStart;

    GameObject player;


    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerYPositionOnStart = player.transform.position.y;
        playerXPositionOnStart = player.transform.position.x;
    }

    private void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, cameraTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, cameraTimeY);
        transform.position = new Vector3(posX, posY, transform.position.z);

        if(cameraBounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, playerXPositionOnStart + 9f, playerXPositionOnStart + 340f),
                                             Mathf.Clamp(transform.position.y, playerYPositionOnStart + 4f, playerYPositionOnStart + 10f),
                                             Mathf.Clamp(transform.position.z, -10, -10));
        }
    }
}
