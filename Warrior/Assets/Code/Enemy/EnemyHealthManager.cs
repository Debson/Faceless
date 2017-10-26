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
    private Image enemyHealthBar;

    private static int enemyHealth;
    //[SerializeField]
    //public GameObject deathEffect;
    private bool pointsAdded;
    private float enemyHeight;
    private static int maxHealth;
    GameObject enemyHealthBarCanvas;
    protected void Awake()
    {
        enemyHealth = enemyMaxHealth;
        enemyHeight = GetComponent<Collider2D>().bounds.size.y;
        maxHealth = enemyMaxHealth;
    }

    void Update()
    {
        Debug.Log(enemyHealth);
        if (enemyHealth <= 0 && !pointsAdded)
        {
            enemyHealth = 0;
            ScoreManager.AddPoints(pointsToAdd);
            pointsAdded = true;
            
            Destroy(gameObject, t: 0);
        }
  
        enemyHealthBar.transform.position = new Vector3(transform.position.x, transform.position.y + enemyHeight, transform.position.z);
    }

    public static void GiveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
    }
    
    public static int GetHealth()
    {
        return enemyHealth;
    }

    public static int GetMaxHealth()
    {
        return maxHealth;
    }
}
