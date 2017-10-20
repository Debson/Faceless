using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnContact : MonoBehaviour {


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        { 
            var player = GetComponent<WalkMovement>();
            player.knockbackTimeCount = player.knockBackLength;

            if (transform.position.x < collision.transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        }
    }
}
