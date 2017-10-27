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

    EnemyHealthManager enemyHealthManager;
    private float lastAmount;
    private float currentAmount;

    protected void Awake()
    {
        enemyHealthManager = GetComponent<EnemyHealthManager>();
    }

    protected void Start()
    {
        lastAmount = content.fillAmount;
    }

    protected void Update()
    {
        if (enemyHealthManager.GetHealth() <= 0)
        {
            content.fillAmount = 0;
        }

        HandleBar();
    }

    private void HandleBar()
    {
        currentAmount = Map(enemyHealthManager.GetHealth(), 0, enemyHealthManager.GetMaxHealth(), 0, 1);
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
