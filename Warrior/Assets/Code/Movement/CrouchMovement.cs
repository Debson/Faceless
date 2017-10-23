using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchMovement : MonoBehaviour
{
    Collider2D crouchTrigger;

    public bool crouchRequest;

    private float crouchTimer = 0;
    private float crouchCooldown = 0.25f;

    public bool isCrouching
    {
        get; private set;
    }


    protected void Awake()
    {
        crouchTrigger = GetComponent<Collider2D>();
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Crouch") && !EnterTerritory.IsCharacterControlEnabled)
        {
            isCrouching = true;
            crouchTimer = crouchCooldown;
            crouchTrigger.enabled = false;
        }

        if (isCrouching)
        {
            if (crouchTimer > 0)
            {
                crouchTimer -= Time.deltaTime;
            }
            else
            {
                isCrouching = false;
                crouchTrigger.enabled = true;
            }
        }
    }
}
