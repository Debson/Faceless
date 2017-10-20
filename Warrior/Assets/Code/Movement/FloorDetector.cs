﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FloorDetector : MonoBehaviour
{
    public bool isTouchingFloor
    {
        get; private set;
    }

    public float? distanceToFloor
    {
        get; private set;
    }

    public Vector2? floorUp
    {
        get; private set;
    }

    public Collider2D feetCollider
    {
        get; private set;
    }

    [SerializeField]
    ContactFilter2D floorFilter;

    static readonly Collider2D[] tempColliderList = new Collider2D[3];
    static readonly RaycastHit2D[] tempHitList = new RaycastHit2D[1];

    protected void Awake()
    {
        feetCollider = GetComponent<Collider2D>();
    }

    protected void FixedUpdate()
    {
        Collider2D floorWeAreStandingOn = DetectTheFloorWeAreStandingOn();
        isTouchingFloor = floorWeAreStandingOn != null;
        /*
        if(floorWeAreStandingOn != null)
        {
            distanceToFloor = 0;
        }
        else
        {
            floorUp = null;
            RaycastHit2D? floorUnderUs = DetectFloorUnderUs();

            if(floorUnderUs != null)
            {
                distanceToFloor = floorUnderUs.Value.distance;
            }
            else
            {
                distanceToFloor = null;
            }
        }*/
    }
    /*
    RaycastHit2D? DetectFloorUnderUs()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, floorFilter, tempHitList) > 0)
        {
            return tempHitList[0];
        }

        return null;
    }*/

    Collider2D DetectTheFloorWeAreStandingOn()
    {
        int foundColliderCount = Physics2D.OverlapCollider(feetCollider, floorFilter, tempColliderList);

        for(int i = 0; i < foundColliderCount; i++)
        {
            Collider2D collider = tempColliderList[i];
            ColliderDistance2D distance = collider.Distance(feetCollider);

            if(distance.distance >= -.1f && Vector2.Dot(Vector2.up, distance.normal) > 0)
            {
                return collider;
            }
        }

        return null;
    }

}
