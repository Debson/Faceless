using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillOnContactWith : MonoBehaviour
{
    [SerializeField]
    LayerMask layersToKill;

    protected void Start()
    {
        
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void TryToKill(GameObject gameObjectWeHit)
    {
        if (enabled == false)
        {
            return;
        }
        
        if(layersToKill.Includes(gameObjectWeHit.layer))
        {
            
        }

    }
}
