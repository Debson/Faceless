using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemyOnContact : MonoBehaviour
{
    [SerializeField]
    public int damageToGive;

    [SerializeField]
    public float knockbackStrenght;

    [SerializeField]
    public float knockBackLength;

    EnemyHealthManager enemyHealthManager;
    SpriteRenderer spriteRenderer;
    Color startColor;

    private float knockbackTimeCount = 0.2f;
    private bool knockFromRight;
    private bool hitOnlyOnce;
    
    protected void Awake()
    {
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        startColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AttackTrigger")
        {
            if (!hitOnlyOnce)
            {
                EnemyHealthManager.GiveDamage(damageToGive);
            }
            hitOnlyOnce = true;

            knockbackTimeCount = knockBackLength;

            if (collision.transform.position.x > transform.position.x)
            {
                knockFromRight = true;
            }
            else
            {
                knockFromRight = false;
            }
        }
    }

    protected void FixedUpdate()
    {
        if (knockbackTimeCount <= 0)
        {
            spriteRenderer.color = startColor;
            hitOnlyOnce = false;
            //gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (knockFromRight)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - (Time.deltaTime * knockbackStrenght) , gameObject.transform.position.y);
                spriteRenderer.color = new Color(255, 0, 0);
                //gameObject.transform.rotation = Quaternion.Euler(0, 0, -40);
            }
            if (!knockFromRight)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + (Time.deltaTime * knockbackStrenght), gameObject.transform.position.y);
                spriteRenderer.color = new Color(255, 0, 0);
                //gameObject.transform.rotation = Quaternion.Euler(0, 0, -40);
            }
        }
        knockbackTimeCount -= Time.deltaTime;
    }
}
