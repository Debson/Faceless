using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkMovement : MonoBehaviour
{
    public float desiredWalkDirection;
    public static int playerPoints = 0;

    public float knockback;
    public float knockBackLength;
    public float knockbackTimeCount;
    public bool knockFromRight;

    [SerializeField]
    float walkSpeed = 50;

    Rigidbody2D myBody;
    CrouchMovement crouchMovement;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        crouchMovement = GetComponent<CrouchMovement>();
    }

    protected void FixedUpdate()
    {
        float desiredXVelocity;

        if (crouchMovement.isCrouching)
        {
            desiredXVelocity = desiredWalkDirection * walkSpeed * 0.4f * Time.deltaTime;
        }
        else
        {
            desiredXVelocity = desiredWalkDirection * walkSpeed * Time.deltaTime;
        }
       
        if (knockbackTimeCount <= 0)
        {
            myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
        }
        else
        {
            if(knockFromRight)
            {
                myBody.velocity = new Vector2(-knockback, knockback / 3);
            }
            if(!knockFromRight)
            {
                myBody.velocity = new Vector2(knockback, knockback / 3);
            }

            knockbackTimeCount -= Time.deltaTime;
        }

        
    }

}
