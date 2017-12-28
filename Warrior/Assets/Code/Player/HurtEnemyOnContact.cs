using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemyOnContact : MonoBehaviour
{
    [SerializeField]
    public float firstAttackMultiplier = 0.2f;

    [SerializeField]
    public float secondAttackMultiplier = 0.3f;

    [SerializeField]
    public float thirdAttackMultiplier = 0.5f;

    [SerializeField]
    public float specialAttackMultiplier = 1f;

    [SerializeField]
    private float shortenSpecialAttackCooldownBy = 0.5f;

    [SerializeField]
    public float knockbackStrenght = 5f;

    [SerializeField]
    public float knockbackStrenghtOnSpecialAttack = 20f;

    [SerializeField]
    public float knockBackLength = 0.1f;

    [SerializeField]
    public bool comboEnabled = true;

    [SerializeField]
    public float stunTime = 0.7f;

    [SerializeField]
    private bool paintWhiteOnHit = true;

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
    PlayerController player;

    public delegate void OnHitStun(float time);
    public static event OnHitStun onStun;
    public static event OnHitStun onStunStop;

    public bool knockbackFinished { get; private set; }
    public bool hitOnlyOnce { get; set; }
    public bool isHurt { get; set; }

    private GameObject bloodInstantiate;
    private ParticleSystem bl;
    private Texture damageFlash;

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
        player = FindObjectOfType<PlayerController>();
        startShader = spriteRenderer.material.shader;
    }

    protected void Start()
    {
        startColor = spriteRenderer.color;
        var main = bl.main;
        main.startColor = new Color(bloodColor.r, bloodColor.g, bloodColor.b);
    }

    protected void LateUpdate()
    {
        if (onStun != null)
        {
            onStun(stunTime);
            onStun -= wanderWalkController.StunEnemy;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AttackTrigger")
        {
            attackMovement.ShortenSpecialAttackTimeLeft(shortenSpecialAttackCooldownBy);
            if (wanderWalkController != null)
            {
                onStun += wanderWalkController.StunEnemy;
            }
            CheckPlayerState(collision);
        }
    }

    private void CheckPlayerState(Collider2D collision)
    {
        if (!hitOnlyOnce)
        {
            switch (attackMovement.attackCount)
            {
                case 0:
                    {//Normal attack
                        enemyHealthManager.GiveDamage((int)(enemyHealthManager.GetMaxHealth() * firstAttackMultiplier));
                        audioManager.monsterHurt[0].Play();
                        StartCoroutine(Knockback(knockBackLength, knockbackStrenght, false));
                        break;
                    }
                case 1:
                    {//Normal first combo attack
                        enemyHealthManager.GiveDamage((int)(enemyHealthManager.GetMaxHealth() * firstAttackMultiplier));
                        audioManager.monsterHurt[1].Play();
                        StartCoroutine(Knockback(knockBackLength, knockbackStrenght, false));
                        break;
                    }
                case 2:
                    {//Normal second combo attack
                        enemyHealthManager.GiveDamage((int)(enemyHealthManager.GetMaxHealth() * secondAttackMultiplier));
                        audioManager.monsterHurt[2].Play();
                        StartCoroutine(Knockback(knockBackLength, knockbackStrenght, false));
                        break;
                    }
                case 3:
                    {//Normal third combo attack
                        enemyHealthManager.GiveDamage((int)(enemyHealthManager.GetMaxHealth() * thirdAttackMultiplier));
                        audioManager.monsterHurt[2].Play();
                        StartCoroutine(Knockback(knockBackLength + 0.1f, knockbackStrenght * 5f, false));
                        break;
                    }
                case 4:
                    {//Special attack form ground
                        enemyHealthManager.GiveDamage((int)(enemyHealthManager.GetMaxHealth() * specialAttackMultiplier));
                        audioManager.monsterHurt[2].Play();
                        StartCoroutine(Knockback(knockBackLength + 0.1f, knockbackStrenght, true));
                        break;
                    }
                case 5:
                    {//Special attack from air
                        enemyHealthManager.GiveDamage((int)(enemyHealthManager.GetMaxHealth() * specialAttackMultiplier));
                        audioManager.monsterHurt[2].Play();
                        StartCoroutine(Knockback(knockBackLength + 0.2f, knockbackStrenghtOnSpecialAttack, false));
                        break;
                    }
            }
            StartCoroutine(EnemyStunDelay());

            bloodInstantiate = Instantiate(blood, new Vector2(transform.position.x, transform.position.y + enemyYBounds * 0.7f), Quaternion.identity);
            Destroy(bloodInstantiate, 2f);
        }
        hitOnlyOnce = true;
        if (collision.transform.position.x > transform.position.x)
        {
            knockFromRight = true;
        }
        else
        {
            knockFromRight = false;
        }
    }

    IEnumerator Knockback(float time, float strength, bool specialAttack)
    {
        var startTime = time;
        while (time >= 0)
        {
            knockbackFinished = false;
            if (knockFromRight)
            {
                if (specialAttack)
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (Time.deltaTime * strength * 14f));
                }
                else
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                }
                if (paintWhiteOnHit && (time > (startTime - 0.1f)))
                {
                    spriteRenderer.material.shader = Shader.Find("PaintWhite");
                }
                time -= Time.deltaTime;
                yield return null;
            }

            if (!knockFromRight)
            {
                if (specialAttack)
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (Time.deltaTime * strength * 14f));
                }
                else
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x + (Time.deltaTime * strength), gameObject.transform.position.y);
                }
                if (paintWhiteOnHit && (time > (startTime - 0.1f)))
                {
                    spriteRenderer.material.shader = Shader.Find("PaintWhite");
                }
                time -= Time.deltaTime;
                yield return null;
            }

            if(time < (startTime - 0.1f))
            {//Sprite should only blink with white
                spriteRenderer.material.shader = startShader;
            }
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        hitOnlyOnce = false;
        knockbackFinished = true;
    }

    IEnumerator EnemyStunDelay()
    {
        isHurt = true;
        yield return new WaitForSeconds(stunTime);
        isHurt = false;
    }
}
