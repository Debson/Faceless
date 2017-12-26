using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private float timeToShakeFor = 1.5f;

    [SerializeField]
    private float maxTimeBetweenShakes = 0.2f;

    [SerializeField]
    private float shakeMagnitude = 1;

    [SerializeField]
    private float timeToShakeForAttack = 0.2f;

    [SerializeField]
    private float maxTimeBetweenShakesAttack;

    private DragonController dragonController;
    private TurnAround player;
    private WalkMovement walkMovement;

    public bool shakeScreen { get; set; }
    public bool shakeScreenOnAttack { get; set; }

    private float direction;

    private bool callOnce;

    protected void Awake()
    {
        dragonController = FindObjectOfType<DragonController>();
        player = FindObjectOfType<TurnAround>();
        walkMovement = FindObjectOfType<WalkMovement>();
    }

    protected void LateUpdate()
    {
        if (shakeScreen && !callOnce)
        {
            StartCoroutine(ShakeCamera());
            callOnce = true;
        }
    }

    public void ShakeOnFirstAtack()
    {
        StartCoroutine(ShakeCameraOnFirstAttack());
        AttackMovement.onFirstAttack -= ShakeOnFirstAtack;
    }
    public void ShakeOnSecondAttack()
    {
        StartCoroutine(ShakeCameraOnSecondAttack());
        AttackMovement.onSecondAttack -= ShakeOnSecondAttack;
    }

    public void ShakeOnThirdAttack()
    {
        StartCoroutine(ShakeCameraOnThirdAttack());
        AttackMovement.onThirdAttack -= ShakeOnThirdAttack;
    }

    public void ShakeOnHurt()
    {
        StartCoroutine(ShakeCameraOnHurt());
        HurtPlayerOnContact.onHurt -= ShakeOnHurt;
    }

    IEnumerator ShakeCamera()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.localPosition;

        float timePassed = 0;
        while(timePassed < timeToShakeFor)
        {
            float percentComplete = timePassed / timeToShakeFor;
            percentComplete *= 2;
            if(percentComplete > 1)
            {
                percentComplete = 2 - percentComplete ;
            }
            Vector2 deltaPosition = Random.insideUnitCircle * shakeMagnitude * percentComplete;
            camera.transform.localPosition = startingPosition + (Vector3)deltaPosition;

            float maxTime = maxTimeBetweenShakes * (1 - percentComplete);
            float sleepTime = Random.Range(0, maxTime);
            yield return new WaitForSeconds(sleepTime);
            sleepTime = Mathf.Max(Time.deltaTime, sleepTime);
            timePassed += sleepTime;
        }

        camera.transform.localPosition = startingPosition;

        shakeScreen = false;
        callOnce = false;
    }

    IEnumerator ShakeCameraOnFirstAttack()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.localPosition;

        float timePassed = 0;
        float x = 0;
        if(player.isFacingLeft)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        while(timePassed < timeToShakeForAttack)
        {
            Vector2 deltaPosition = new Vector2((Mathf.Pow(2, -16 * x) * Mathf.Sin((x - 0.05f / 16f) * (0.2f * Mathf.PI) / 0.6f) * 30f) * direction,
                                                Random.insideUnitCircle.y / 10f);
            camera.transform.localPosition = startingPosition + (Vector3)deltaPosition;

            float maxTime = maxTimeBetweenShakesAttack * Time.deltaTime;
            float sleepTime = Random.Range(0, maxTime);
            yield return new WaitForSeconds(sleepTime);
            timePassed += Time.deltaTime;
            x += Time.fixedDeltaTime * 2.1f;
        }
        camera.transform.localPosition = startingPosition;
    }

    IEnumerator ShakeCameraOnSecondAttack()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.localPosition;

        float timePassed = 0;
        float x = 0;
        if (player.isFacingLeft)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        while (timePassed < timeToShakeForAttack)
        {
            Vector2 deltaPosition = new Vector2(EasingFunction.EaseOutCirc(0.01f, 0.7f, x) * direction,
                                                EasingFunction.EaseInBack(0.01f, 0.4f, x));

            camera.transform.localPosition = startingPosition + (Vector3)deltaPosition;

            float maxTime = maxTimeBetweenShakesAttack * Time.deltaTime;
            float sleepTime = Random.Range(0, maxTime);
            yield return new WaitForSeconds(sleepTime);
            timePassed += Time.deltaTime;
            x += Time.fixedDeltaTime * 2.1f;
        }
        camera.transform.localPosition = startingPosition;
    }

    IEnumerator ShakeCameraOnThirdAttack()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.localPosition;

        float timePassed = 0;
        float x = 0;
        if (player.isFacingLeft)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        while (timePassed < timeToShakeForAttack)
        {
            Vector2 deltaPosition = new Vector2(0f, EasingFunction.SpringD(0.01f, -0.3f, x));

            camera.transform.localPosition = startingPosition + (Vector3)deltaPosition;

            //float maxTime = maxTimeBetweenShakesAttack * Time.deltaTime;
            //float sleepTime = Random.Range(0, maxTime);
            timePassed += Time.deltaTime;
            x += Time.fixedDeltaTime * 2.1f;
            yield return null;
        }
        camera.transform.localPosition = startingPosition;
    }

    IEnumerator ShakeCameraOnHurt()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.localPosition;

        float timePassed = 0;
        float x = 0;
        if (player.isFacingLeft)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        while (timePassed < timeToShakeForAttack)
        {
            Vector2 deltaPosition = new Vector2(EasingFunction.EaseOutBack(0.01f, 0.6f * walkMovement.GetKnockbackDirection(), x), 0f);

            camera.transform.localPosition = startingPosition + (Vector3)deltaPosition;

            //float maxTime = maxTimeBetweenShakesAttack * Time.deltaTime;
            //float sleepTime = Random.Range(0, maxTime);
            timePassed += Time.deltaTime;
            x += Time.fixedDeltaTime * 2.1f;
            yield return null;
        }
        camera.transform.localPosition = startingPosition;
    }
}
