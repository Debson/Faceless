using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TurnAround : MonoBehaviour
{
    static readonly Quaternion flipRotation = Quaternion.Euler(0, 180, 0);

    Rigidbody2D myBody;
    FloorDetector floorDetector;
    WalkMovement walkMovement;

    public bool isFacingLeft { get; set; }

    public bool IsFacingRight { get; set; }

    public float direction { get; private set; }

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        floorDetector = GetComponent<FloorDetector>();
        walkMovement = GetComponent<WalkMovement>();
    }

    protected void Start()
    {
        IsFacingRight = true;
    }

    protected void Update()
    {
        direction = isFacingLeft ? -1: 1;

        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && 
            !Input.GetKey(KeyCode.LeftArrow) && isFacingLeft)
        {
            transform.rotation *= flipRotation;
            isFacingLeft = false;
            IsFacingRight = true;
        }

        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && 
            !Input.GetKey(KeyCode.RightArrow) && IsFacingRight)
        {
            transform.rotation *= flipRotation;
            IsFacingRight = false;
            isFacingLeft = true;
        }
    }

    protected void FixedUpdate()
    {
        float xVelocity = myBody.velocity.x;
        if (walkMovement.knockbackFinished)
        {
            if (Mathf.Abs(xVelocity) > 0.05f)
            {
                bool isTravelingLeft = xVelocity < 0;
                IsFacingRight = !isTravelingLeft;
                if (isFacingLeft != isTravelingLeft)
                {
                    isFacingLeft = isTravelingLeft;
                    transform.rotation *= flipRotation;
                }
            }
        }
    }
}
