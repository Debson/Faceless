using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpMovement : MonoBehaviour
{
    public bool jumpRequest;

    [SerializeField]
    float jumpSpeed = 7f;

    Rigidbody2D myBody;
    FloorDetector floorDetector;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        floorDetector = GetComponent<FloorDetector>();
    }

    protected void FixedUpdate()
    {
        if(jumpRequest && floorDetector.isTouchingFloor)
        {
            myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }

        jumpRequest = false;
    }

}
