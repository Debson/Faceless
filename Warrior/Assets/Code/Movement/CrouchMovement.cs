using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchMovement : MonoBehaviour
{
    Collider2D crouchTrigger;
    PlayerController playerController;

    public bool crouchRequest { get; set; }

    private float crouchTimer = 0;
    private float crouchCooldown = 0.25f;

    public bool isCrouching
    {
        get; private set;
    }


    protected void Awake()
    {
        crouchTrigger = GetComponent<Collider2D>();
        playerController = GetComponent<PlayerController>();
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Crouch") && !playerController.CharacterControlDisabled)
        {
            isCrouching = true;
            crouchTimer = crouchCooldown;
            //crouchTrigger.enabled = false;
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
                //crouchTrigger.enabled = true;
            }
        }
    }
}
