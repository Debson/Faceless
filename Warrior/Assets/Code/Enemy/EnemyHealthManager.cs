using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField]
    public int enemyMaxHealth;

    [SerializeField]
    public int pointsToAdd;

    private static int enemyHealth;
    //[SerializeField]
    //public GameObject deathEffect;
    private bool pointsAdded;

    protected void Awake()
    {
        enemyHealth = enemyMaxHealth;
    }

    void Update()
    {
        //Debug.Log(enemyHealth);
        if (enemyHealth <= 0 && !pointsAdded)
        {
            ScoreManager.AddPoints(pointsToAdd);
            pointsAdded = true;
            
            Destroy(gameObject, t: 0);
        }
    }

    public static void GiveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
    }
    
    public static int GetHealth()
    {
        return enemyHealth;
    }
}
