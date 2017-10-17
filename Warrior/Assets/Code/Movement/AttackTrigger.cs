using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackTrigger : MonoBehaviour
{
    public Collider2D attackTrigger;

    protected void Awake()
    {
        attackTrigger = GetComponent<Collider2D>();
        attackTrigger.enabled = false;
    }

    

}
