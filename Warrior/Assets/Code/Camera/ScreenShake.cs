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

    public bool shakeScreen { get; set; }
    public bool shakeScreenOnAttack { get; set; }

    private float direction;

    private bool callOnce;

    protected void Awake()
    {
        dragonController = FindObjectOfType<DragonController>();
        player = FindObjectOfType<TurnAround>();
    }

    protected void LateUpdate()
    {
        if (shakeScreen && !callOnce)
        {
            StartCoroutine(ShakeCamera());
            callOnce = true;
        }

        if(shakeScreenOnAttack && !callOnce)
        {
            StartCoroutine(ShakeCameraOnAttack());
            callOnce = true;
        }
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

    IEnumerator ShakeCameraOnAttack()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.position;

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
            Vector2 deltaPosition = new Vector2((Mathf.Pow(2, -16 * x) * Mathf.Sin((x - 0.05f / 16f) * (0.2f * Mathf.PI) / 0.6f) * 15f) * direction,
                                                Random.insideUnitCircle.y / 15f);
            camera.transform.position = startingPosition + (Vector3)deltaPosition;

            float maxTime = maxTimeBetweenShakesAttack * Time.deltaTime;
            float sleepTime = Random.Range(0, maxTime);
            yield return new WaitForSeconds(sleepTime);
            timePassed += Time.deltaTime;
            x += Time.fixedDeltaTime * 2.1f;
        }
        camera.transform.position = startingPosition;

        shakeScreenOnAttack = false;
        callOnce = false;

    }

}
