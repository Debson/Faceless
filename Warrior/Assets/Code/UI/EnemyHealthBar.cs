using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private float healthDecrementationTime;

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
        if (EnemyHealthManager.GetHealth() <= 0)
        {
            gameObject.SetActive(false);
        }

        HandleBar();
    }

    private void HandleBar()
    {
        currentAmount = Map(EnemyHealthManager.GetHealth(), 0, EnemyHealthManager.GetMaxHealth(), 0, 1);

        if (currentAmount != lastAmount)
        {
            content.fillAmount = Mathf.Lerp(lastAmount, currentAmount, healthDecrementationTime * Time.fixedDeltaTime);
        }
        lastAmount = content.fillAmount;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
