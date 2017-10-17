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

    GameObject player;


    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, cameraTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, cameraTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if(cameraBounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPosition.x, maxCameraPosition.x),
                                             Mathf.Clamp(transform.position.y, minCameraPosition.y, maxCameraPosition.y),
                                             Mathf.Clamp(transform.position.z, minCameraPosition.z, maxCameraPosition.z));
        }
    }

}
