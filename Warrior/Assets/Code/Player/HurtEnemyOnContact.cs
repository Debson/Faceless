using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemyOnContact : MonoBehaviour
{
    [SerializeField]
    public int minDamageToGive;

    [SerializeField]
    public int maxDamageToGive;

    [SerializeField]
    public float knockbackStrenght;

    [SerializeField]
    public float knockBackLength;

    [SerializeField]
    public bool comboEnabled;

    [SerializeField]
    public float stunTime = 0.7f;

    [SerializeField]
    private bool paintWhiteOnHit;

    [SerializeField]
    private GameObject blood;

    [SerializeField]
    Color bloodColor;

    EnemyHealthManager enemyHealthManager;
    SpriteRenderer spriteRenderer;
    Color startColor;
    AudioManager audioManager;
    Shader startShader;
    AttackMovement attackMovement;
    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;

    public delegate void OnHitStun(float time);
    public static event OnHitStun onStun;
    public static event OnHitStun onStunStop;

    public bool hitOnlyOnce { get; set; }

    public bool isHurt { get; set; }

    private GameObject bloodInstantiate;
    private ParticleSystem bl;
    private Texture damageFlash;

    private float knockbackTimeCount = 0.2f;
    private float enemyYBounds;

    private bool knockFromRight;
    private bool startCombo;


    protected void Awake()
    {
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bl = blood.GetComponent<ParticleSystem>();
        enemyYBounds = GetComponent<Collider2D>().bounds.size.y;
        audioManager = FindObjectOfType<AudioManager>();
        attackMovement = FindObjectOfType<AttackMovement>();
        wanderWalkController = FindObjectOfType<WanderWalkController>();
        hurtPlayerOnContact = FindObjectOfType<HurtPlayerOnContact>();
        startShader = spriteRenderer.material.shader;
    }

    protected void Start()
    {
        startColor = spriteRenderer.color;
        var main = bl.main;
        main.startColor = new Color(bloodColor.r, bloodColor.g, bloodColor.b);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AttackTrigger")
        {
            onStun += wanderWalkController.StunEnemy;

            if (!hitOnlyOnce)
            {
                switch (attackMovement.attackCount)
                {
                    case 0:
                        {
                            audioManager.monsterHurt[0].Play();
                            break;
                        }
                    case 1:
                        {
                            audioManager.monsterHurt[1].Play();
                            break;
                        }
                    case 2:
                        {
                            audioManager.monsterHurt[2].Play();
                            break;
                        }
                }

                enemyHealthManager.GiveDamage(Random.Range(minDamageToGive, maxDamageToGive));

                StartCoroutine(EnemyStunDelay());

                bloodInstantiate = Instantiate(blood, new Vector2(transform.position.x, transform.position.y + enemyYBounds * 0.7f), Quaternion.identity);
                Destroy(bloodInstantiate, 2f);
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

    protected void LateUpdate()
    {
        if(onStun != null)
        {
            onStun(stunTime);
            onStun -= wanderWalkController.StunEnemy;
        }

        if (knockbackTimeCount <= 0)
        {
            //spriteRenderer.color = startColor;
            spriteRenderer.material.shader = startShader;
            hitOnlyOnce = false;
        }
        else
        {
            if (knockFromRight)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - (Time.deltaTime * knockbackStrenght), gameObject.transform.position.y);
                if (paintWhiteOnHit)
                {
                    spriteRenderer.material.shader = Shader.Find("PaintWhite");
                }
            }
            if (!knockFromRight)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + (Time.deltaTime * knockbackStrenght), gameObject.transform.position.y);
                if (paintWhiteOnHit)
                {
                    spriteRenderer.material.shader = Shader.Find("PaintWhite");
                }
            }
        }
        knockbackTimeCount -= Time.deltaTime;
    }

    IEnumerator EnemyStunDelay()
    {
        isHurt = true;
        yield return new WaitForSeconds(stunTime);
        isHurt = false;
    }
}
