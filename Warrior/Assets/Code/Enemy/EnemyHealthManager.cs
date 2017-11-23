using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField]
    private int enemyMaxHealth;

    [SerializeField]
    private int pointsToAdd;

    [SerializeField]
    private float adjustHealthBarY = 0f;

    [SerializeField]
    private float adjustHealthBarX = 0f;

    [HideInInspector]
    public Image healthBar;


    private bool pointsAdded;
    private int enemyHealth;
    private float enemyHeight;


    protected void Awake()
    {
        enemyHealth = enemyMaxHealth;
        enemyHeight = GetComponent<Collider2D>().bounds.size.y;
        healthBar = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (enemyHealth <= 0 && !pointsAdded)
        {
            enemyHealth = -1;
            ScoreManager.AddPoints(pointsToAdd);
            pointsAdded = true;
        }
        // Enemy must have bottom pivot;
        healthBar.transform.position = new Vector3(transform.position.x + adjustHealthBarX, transform.position.y + enemyHeight + adjustHealthBarY, transform.position.z);
    }

    public void GiveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
    }
    
    public int GetHealth()
    {
        return enemyHealth;
    }

    public int GetMaxHealth()
    {
        return enemyMaxHealth;
    }
}
