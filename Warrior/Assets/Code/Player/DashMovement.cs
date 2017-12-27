using System.Collections;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float dashTime = 0.2f;

    [SerializeField]
    private float dashCooldown = 2f;

    [SerializeField]
    private Sprite dashSpriteRight;

    [SerializeField]
    private Sprite dashSpriteLeft;


    TurnAround player;
    FloorDetector floorDetector;
    PlayerController playerController;
    AudioManager audioManager;
    Rigidbody2D rBody;
    TurnAround turnAround;
    AttackMovement attackMovement;

    private GameObject[] glow;

    /// <summary>
    /// If dash button pressed, do dash
    /// </summary>
    public bool dash { get; set; }
    /// <summary>
    /// Animation bool setter
    /// </summary>
    public bool dashActive { get; set; }

    private int glowCount;
    private int transparencyCount;

    private float time;
    private float startingTime = 1.2f;

    private bool callOnce;
    private bool playOnce;
    private bool createGlow;
    private bool active;
    private bool cooldownIsUp;
    private bool glowFinished;

    protected void Awake()
    {
        player = GetComponent<TurnAround>();
        rBody = GetComponent<Rigidbody2D>();
        floorDetector = GetComponent<FloorDetector>();
        playerController = GetComponent<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        turnAround = GetComponent<TurnAround>();
        attackMovement = GetComponent<AttackMovement>();
    }

    protected void Start()
    {
        glow = new GameObject[5];
        transparencyCount = 1;
        glowCount = 1;
        time = startingTime;
        createGlow = true;
        cooldownIsUp = true;
    }

    protected void FixedUpdate()
    {
        if (dash && floorDetector.isTouchingFloor && cooldownIsUp)
        {
            playerController.CharacterControlDisabled = true;
            attackMovement.AttackMovementEnabled = false;
            StartCoroutine(Dash());
        }
        ChangeTransparencyToZero();
    }

    IEnumerator Dash()
    {
        if (createGlow)
        {
            StartCoroutine(CreateGlow());
        }
        createGlow = false;

        if(!playOnce)
        {
            audioManager.playerDash[Random.Range(0, 3)].Play();
            playOnce = true;
        }

        //transform.position = new Vector2(transform.position.x + Time.fixedDeltaTime * speed * turnAround.direction, transform.position.y);
        dashActive = true;

        Physics2D.IgnoreLayerCollision(10, 8, true);
        rBody.AddForce(Vector2.right * 2200f * turnAround.direction);
        yield return new WaitForSeconds(dashTime);
        Physics2D.IgnoreLayerCollision(10, 8, false);

        dashActive = false;
        dash = false;
        cooldownIsUp = false;
        playOnce = false;
        playerController.CharacterControlDisabled = false;
    }

    IEnumerator CreateGlow()
    {
        for (glowCount = 1; glowCount < 5; glowCount++)
        {
            if (floorDetector.isTouchingFloor)
            {
                if ((glowCount > 1 || floorDetector.isTouchingFloor) && !callOnce)
                {
                    //When glowcount is greater than 2, start process of decreasing color.a value
                    active = true;
                    callOnce = true;
                }
                glow[glowCount - 1] = new GameObject();
                glow[glowCount - 1].transform.position = transform.position;
                glow[glowCount - 1].transform.localScale = new Vector2(glow[glowCount - 1].transform.localScale.x * 6f, glow[glowCount - 1].transform.localScale.y * 6f);
                glow[glowCount - 1].AddComponent<SpriteRenderer>();
                var renderer = glow[glowCount - 1].GetComponent<SpriteRenderer>();
                if (player.isFacingLeft)
                {
                    renderer.sprite = dashSpriteLeft;
                }
                else
                {
                    renderer.sprite = dashSpriteRight;
                }
                renderer.color = new Color(1, 1, 1, EasingFunction.EaseOutExpo(0.5f, 0.55f, glowCount / 20f));
                yield return new WaitForSeconds(dashTime * speed * 0.004f);
            }
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        cooldownIsUp = true;
    }

    private void ChangeTransparencyToZero()
    {
        if (active && transparencyCount < 5)
        {
            if (glow[transparencyCount - 1] == null)
            {
                return;
            }
            else
            {
                var renderer = glow[transparencyCount - 1].GetComponent<SpriteRenderer>();

                renderer.color = new Color(1, 1, 1, renderer.color.a - EasingFunction.EaseInQuint(-1f, 0.4f, time) * Time.deltaTime);
                if (renderer.color.a < 0)
                {
                    Destroy(glow[transparencyCount - 1]);
                    transparencyCount++;
                    time += 0.2f;
                }
            }
        }

        if(transparencyCount > 4)
        {
            active = false;
            glowCount = 1;
            transparencyCount = 1;
            createGlow = true;
            time = startingTime;
            callOnce = false;
            attackMovement.AttackMovementEnabled = true;
            StartCoroutine(DashCooldown());
        }
    }
}
