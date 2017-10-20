using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillOnContactWith : MonoBehaviour
{
    [SerializeField]
    LayerMask layersToKill;

    [SerializeField]
    GameObject bat;

    protected void Awake()
    {
       // bat = GameObject.FindGameObjectWithTag("Bat");
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        TryToKill(collision.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        TryToKill(collision.gameObject);
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
