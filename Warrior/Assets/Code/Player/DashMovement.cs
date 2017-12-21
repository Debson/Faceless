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

    protected void Awake()
    {
        player = GetComponent<TurnAround>();
        floorDetector = GetComponent<FloorDetector>();
        playerController = GetComponent<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    protected void Start()
    {
        glow = new GameObject[4];
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
            playerController.CharacterControlEnabled = true;
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
            //audioManager.playerDash[Random.Range(0, 3)].Play();
            playOnce = true;
        }

        if (player.isFacingLeft)
        {
            transform.position = new Vector2(transform.position.x - Time.fixedDeltaTime * speed, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + Time.fixedDeltaTime * speed, transform.position.y);
        }
        dashActive = true;
        Physics2D.IgnoreLayerCollision(10, 8, true);
        yield return new WaitForSeconds(dashTime);
        dashActive = false;
        Physics2D.IgnoreLayerCollision(10, 8, false);
        dash = false;
        cooldownIsUp = false;
        playOnce = false;
        playerController.CharacterControlEnabled = false;
    }

    IEnumerator CreateGlow()
    {
        if (glowCount < 5)
        {
            if ((glowCount > 3 || floorDetector.isTouchingFloor) && !callOnce)
            {
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
            renderer.color = new Color(1, 1, 1, (1f / (2 * Mathf.PI)) * (float)glowCount);
            glowCount++;
            yield return new WaitForSeconds(dashTime * speed * 0.002f);
            createGlow = true;
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

                renderer.color = new Color(1, 1, 1, renderer.color.a - EasingFunction.EaseInQuint(0f, 1f, time) * Time.deltaTime);
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
            StartCoroutine(DashCooldown());
        }
    }
}
