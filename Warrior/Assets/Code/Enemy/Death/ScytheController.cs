using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private int minDamageToGive;

    [SerializeField]
    private int maxDamageToGive;

    Rigidbody2D myBody;
    DemonHunter demonHunter;

    private float scytheDirection;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        demonHunter = FindObjectOfType<DemonHunter>();
        scytheDirection = demonHunter.throwScytheDirection;
    }

    protected void Update()
    {
        myBody.velocity = new Vector2(speed * scytheDirection, myBody.velocity.y);
        myBody.transform.rotation *= Quaternion.Euler(0, 0, -speed / 2.7f);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);

            var player = collision.GetComponent<WalkMovement>();
            player.knockbackTimeCount = player.knockBackLength;

            if (collision.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
            Destroy(gameObject, 10f);
        }
    }
}
