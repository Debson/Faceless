using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchMovement : MonoBehaviour
{
    public bool crouchRequest;

    private float crouchTimer = 0;
    private float crouchCooldown = 0.25f;

    public bool isCrouching
    {
        get; private set;
    }

    protected void Awake()
    {
        
    }

    protected void Update()
    {
        if(crouchRequest)
        {
            isCrouching = true;
            crouchTimer = crouchCooldown;
        }
        crouchRequest = false;

        if(isCrouching)
        {
            if (crouchTimer > 0)
            {

                crouchTimer -= Time.deltaTime;
            }
            else
            {
                isCrouching = false;
            }
        }
    }
}
