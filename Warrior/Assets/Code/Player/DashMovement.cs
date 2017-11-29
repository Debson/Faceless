using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float dashTime = 0.2f;

    [SerializeField]
    private Sprite dashSpriteRight;

    [SerializeField]
    private Sprite dashSpriteLeft;


    TurnAround player;
    FloorDetector floorDetector;
    private GameObject[] glow;

    public bool dash { get; set; }
    public bool dashActive { get; set; }

    private int glowCount;
    private int transparencyCount;

    private float time;
    private float timeStart;
    

    private bool callOnce;
    private bool createGlow;
    private bool active;

    protected void Awake()
    {
        player = GetComponent<TurnAround>();
        floorDetector = GetComponent<FloorDetector>();
    }

    protected void Start()
    {
        transparencyCount = 1;
        time = 1.2f;
        glowCount = 1;
        createGlow = true;
        glow = new GameObject[4];
    }

    protected void FixedUpdate()
    {
        //Debug.Log(EasingFunction.EaseOutCubic(0, 1, time));
        //time += Time.deltaTime;
        if (dash && floorDetector.isTouchingFloor)
        {
            StartCoroutine(Dash());
        }
        else
        {

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
    }

    IEnumerator CreateGlow()
    {
        if (glowCount < 5)
        {
            if (glowCount > 3 && !callOnce)
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

    private void ChangeTransparencyToZero()
    {
        if (active && transparencyCount < 5)
        {
            Debug.Log("siema");
            var renderer = glow[transparencyCount - 1].GetComponent<SpriteRenderer>();
            renderer.color = new Color(1, 1, 1, renderer.color.a - EasingFunction.EaseInQuint(0f, 1f, time) * Time.deltaTime);
            Debug.Log(EasingFunction.EaseInQuint(0f, 1f, time) * Time.deltaTime);
            if (renderer.color.a < 0)
            {
                Destroy(glow[transparencyCount - 1]);
                transparencyCount++;
                time += 0.2f;
            }
        }

        if(transparencyCount > 4)
        {
            active = false;
            glowCount = 1;
            transparencyCount = 1;
            createGlow = true;
            time = 1.2f;
            callOnce = false;
        }
    }
}
