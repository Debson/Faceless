using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField]
    public int enemyHealth;

    [SerializeField]
    public int pointsToAdd;

    //[SerializeField]
    //public GameObject deathEffect;
    private bool pointsAdded;
    void Update()
    {

        if (enemyHealth <= 0 && !pointsAdded)
        {
            ScoreManager.AddPoints(pointsToAdd);
            pointsAdded = true;
            
            Destroy(gameObject, t: 1.5f);
        }
        
    }

    public void giveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
    }
}
