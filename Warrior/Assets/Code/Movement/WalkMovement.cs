using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkMovement : MonoBehaviour
{
    public float desiredWalkDirection;
    public static int playerPoints = 0;

    [SerializeField]
    float walkSpeed = 50;

    Rigidbody2D myBody;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        float desiredXVelocity = desiredWalkDirection * walkSpeed * Time.deltaTime;

        myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
    }

}
