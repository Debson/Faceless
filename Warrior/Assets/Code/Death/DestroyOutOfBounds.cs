using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField]
    public float outOfBoundsYPosition = -3;

    protected void FixedUpdate()
    {
        if(transform.position.y < outOfBoundsYPosition)
        {
            Destroy(gameObject);
        }
    }
}
