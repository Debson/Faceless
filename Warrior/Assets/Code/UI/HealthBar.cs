using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image content;
    private float lastAmount;
    private float currentAmount;

    protected void Start()
    {
        lastAmount = content.fillAmount;
    }

    protected void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        currentAmount = Map(HealthManager.GetHealth(), 0, HealthManager.GetMaxHealth(), 0.07f, 0.67f);

        if (currentAmount != lastAmount)
        {
            content.fillAmount = Mathf.Lerp(lastAmount, currentAmount, 2f * Time.fixedDeltaTime);
            Debug.Log(lastAmount);
            Debug.Log(currentAmount);
        }
        lastAmount = content.fillAmount;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
