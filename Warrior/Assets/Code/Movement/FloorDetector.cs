using System.Collections;
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
        get; set;
    }

    public Quaternion? floorRotation { get; private set; }

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
        
        if(floorWeAreStandingOn != null)
        {
            CalculateFloorRotation(floorWeAreStandingOn);
        }
        else
        {
            floorUp = null;
            floorRotation = null;
        }
    }

    private void CalculateFloorRotation(Collider2D floorWeAreStandingOn)
    {
        floorUp = floorWeAreStandingOn.transform.up;
        floorRotation = floorWeAreStandingOn.transform.rotation;
        if(Vector2.Dot(Vector2.up, floorUp.Value) < 0)
        {
            floorUp = -floorUp;
            floorRotation *= Quaternion.Euler(0, 0, 180);
        }
    }

    public Collider2D DetectTheFloorWeAreStandingOn()
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
