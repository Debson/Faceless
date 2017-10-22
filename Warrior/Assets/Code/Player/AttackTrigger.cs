using UnityEngine;


public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    public int damageToGive;


    protected void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            var player = collision.GetComponent<EnemyHealthManager>();

            player.giveDamage(damageToGive);
/*
            knockbackTimeCount = knockBackLength;

            if (collision.transform.position.x < transform.position.x)
            {
                knockFromRight = true;
            }
            else
            {
                knockFromRight = false;
            }*/
        }
        //myBody = collision.GetComponent<Rigidbody2D>();
    }

   /* public void FixedUpdate()
    {

        //float desiredXVelocity;
        if (knockbackTimeCount <= 0)
        {
            //myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
            //spriteRenderer.color = startColor;
        }
        else
        {

            if (knockFromRight)
            {
                myBody.velocity = new Vector2(-knockback, knockback / 3);
                //spriteRenderer.color = new Color(255, 0, 0);

            }
            if (!knockFromRight)
            {
                myBody.velocity = new Vector2(knockback, knockback / 3);
                //spriteRenderer.color = new Color(255, 0, 0);
            }
        }
        knockbackTimeCount -= Time.deltaTime;
    }*/
}
