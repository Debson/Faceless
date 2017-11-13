using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmMovement : MonoBehaviour
{
    [SerializeField]
    Transform arm;

    [SerializeField]
    Transform hand;

    [SerializeField]
    Transform arrow;

    ElfController elfController;

    private bool stretchBow;
    private bool startStretch = true;

    protected void Awake()
    {
        elfController = GetComponent<ElfController>();
    }

    protected void Update()
    {

    }

    public void MoveHandOnShoot()
    {
        if (startStretch)
        {
            arm.transform.rotation *= Quaternion.Euler(0, 0, arm.transform.position.z + 0.1f * 12);
            hand.transform.position = new Vector2(hand.transform.position.x + (Time.fixedDeltaTime * 0.03f * 12 * elfController.direction), hand.transform.position.y);

            if (arm.transform.eulerAngles.z > 20)
            {
                startStretch = false;
                stretchBow = true;
            }
        }

        if (stretchBow)
        {
            arm.transform.rotation *= Quaternion.Euler(0, 0, arm.transform.position.z - 0.1f * 12);
            hand.transform.position = new Vector2(hand.transform.position.x - Time.fixedDeltaTime * (0.08f * 3 * elfController.direction), hand.transform.position.y);

            if (arm.transform.eulerAngles.z > 338 && arm.transform.eulerAngles.z < 342)
            {
                stretchBow = false;
                startStretch = true;
                // Shoot arrow
            }
        }
    }
}
