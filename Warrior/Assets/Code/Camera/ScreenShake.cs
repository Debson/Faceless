using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    float timeToShakeFor = 1.5f;

    [SerializeField]
    float maxTimeBetweenShakes = 0.2f;

    [SerializeField]
    float shakeMagnitude = 1;

    private DragonController dragonController;

    private bool callOnce;

    protected void Awake()
    {
        dragonController = FindObjectOfType<DragonController>();
    }

    protected void LateUpdate()
    {
        if (dragonController.shakeScreen && !callOnce)
        {
            StartCoroutine(ShakeCamera());
            callOnce = true;
        }
    }

    IEnumerator ShakeCamera()
    {
        Camera camera = Camera.main;
        Vector3 startingPosition = camera.transform.position;

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
            camera.transform.position = startingPosition + (Vector3)deltaPosition;

            float maxTime = maxTimeBetweenShakes * (1 - percentComplete);
            float sleepTime = Random.Range(0, maxTime);
            yield return new WaitForSeconds(sleepTime);
            sleepTime = Mathf.Max(Time.deltaTime, sleepTime);
            timePassed += sleepTime;
        }

        camera.transform.position = startingPosition;

        dragonController.shakeScreen = false;
        callOnce = false;
    }

}
