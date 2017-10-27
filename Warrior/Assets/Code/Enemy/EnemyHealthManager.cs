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
    

    private Image healthBar;

    private int enemyHealth;
    //[SerializeField]
    //public GameObject deathEffect;

    private bool pointsAdded;
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
            
            Destroy(gameObject, t: 0.1f);
        }
  
        healthBar.transform.position = new Vector3(transform.position.x, transform.position.y + enemyHeight, transform.position.z);
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
