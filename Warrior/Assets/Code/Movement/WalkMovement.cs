using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkMovement : MonoBehaviour
{
    public float desiredWalkDirection;
    public static int playerPoints = 0;

    public float knockbackStrength;
    public float knockBackLength;
    public float knockbackTimeCount;
    public bool knockFromRight;

    [SerializeField]
    float walkSpeed = 50;

    Rigidbody2D myBody;
    CrouchMovement crouchMovement;
    SpriteRenderer spriteRenderer;
    Color startColor;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        crouchMovement = GetComponent<CrouchMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
       startColor = spriteRenderer.color;
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
            spriteRenderer.color = startColor;
        }
        else
        {
            
            if(knockFromRight)
            {
                myBody.velocity = new Vector2(-knockbackStrength, knockbackStrength / 3);
                spriteRenderer.color = new Color(255, 0, 0);
                
            }
            if(!knockFromRight)
            {
                myBody.velocity = new Vector2(knockbackStrength, knockbackStrength / 3);
                spriteRenderer.color = new Color(255, 0, 0);
            }
            knockbackTimeCount -= Time.deltaTime;
        }
        //knockbackTimeCount -= Time.deltaTime;
    }

}
