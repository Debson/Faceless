using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToAliginWithFloor : MonoBehaviour
{
    static readonly Quaternion flipRotation = Quaternion.Euler(0, 180, 0);

    [SerializeField]
    float lerpSpeedToFloor = 50f;

    [SerializeField]
    float lerpSpeedWhileInAir = 1f;

    FloorDetector floorDetector;
    TurnAround turnAround;

    protected void Awake()
    {
        floorDetector = GetComponent<FloorDetector>();
        turnAround = GetComponent<TurnAround>();
    }

    protected void Update()
    {
        Quaternion rotation;
        float speed;
        if(floorDetector.floorRotation != null)
        {
            rotation = floorDetector.floorRotation.Value;
            speed = lerpSpeedToFloor;
        }
        else
        {
            rotation = Quaternion.identity * Quaternion.Euler(0, 0, -1); ;
            speed = lerpSpeedWhileInAir;
        }

        if(turnAround.isFacingLeft)
        {
            rotation *= flipRotation;
        }

        if (floorDetector.DetectTheFloorWeAreStandingOn() == null)
        {
            return;
        }
        else
        {
            if (floorDetector.DetectTheFloorWeAreStandingOn().tag != "Enemy")
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
        }
    }
}
