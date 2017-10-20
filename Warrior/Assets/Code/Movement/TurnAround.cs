using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TurnAround : MonoBehaviour
{
    static readonly Quaternion flipRotation = Quaternion.Euler(0, 180, 0);

    Rigidbody2D myBody;
    FloorDetector floorDetector;

    public bool isFacingLeft
    {
        set; private get;
    }

    bool isFacingLeft2;
    bool isFacingRight = true;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        floorDetector = GetComponent<FloorDetector>();
    }

    protected void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && 
            !Input.GetKey(KeyCode.LeftArrow) && isFacingLeft)
        {
            transform.rotation *= flipRotation;
            isFacingLeft = false;
            isFacingRight = true;
            //Debug.Log("Pressed1");
        }

        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && 
            !Input.GetKey(KeyCode.RightArrow) && isFacingRight)
        {
            transform.rotation *= flipRotation;
            isFacingRight = false;
            isFacingLeft = true;
            //Debug.Log("Pressed2");

        }
    }

    protected void FixedUpdate()
    {
        
        
        float xVelocity = myBody.velocity.x;

        if (Mathf.Abs(xVelocity) > .05f)
        {
            bool isTravelingLeft = xVelocity < 0;
            isFacingRight = !isTravelingLeft;
            if(isFacingLeft != isTravelingLeft)
            {
                isFacingLeft = isTravelingLeft;
                transform.rotation *= flipRotation;
            }
        }
    }
}
