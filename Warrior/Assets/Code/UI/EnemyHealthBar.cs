using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private float fillAmount = 1f;

    [SerializeField]
    private float healthDecrementationTime = 4f;

    [SerializeField]
    private bool setAutoBarSize;

    [SerializeField]
    private Image content;

    EnemyHealthManager enemyHealthManager;
    Canvas healthBarCanvas;
    Collider2D enemyCollider;

    private bool isFacingLeft;
    private bool isFacingRight;

    private float lastAmount;
    private float currentAmount;


    protected void Awake()
    {
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        healthBarCanvas = GetComponentInChildren<Canvas>();
        enemyCollider = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        if (setAutoBarSize)
        {
            healthBarCanvas.transform.localScale = new Vector2(enemyCollider.bounds.size.x * 0.8f, enemyCollider.bounds.size.x * 1.1f);
        }
        lastAmount = content.fillAmount;
    }

    protected void Update()
    {

        if (enemyHealthManager.GetHealth() <= 0)
        {
            content.fillAmount = 0;
            healthBarCanvas.enabled = false;
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
