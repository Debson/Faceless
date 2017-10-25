using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private int maxPlayerHealth;

    private static int maxHealth;
    private static int playerHealth;

    LifeManager lifeManager;

    protected void Awake()
    {
        lifeManager = GetComponent<LifeManager>();
    }

    protected void Start()
    {
        playerHealth = maxPlayerHealth;
        maxHealth = maxPlayerHealth;
    }

    protected void Update()
    {
        //Debug.Log(playerHealth);
        if(playerHealth <= 0)
        {
            //Time.timeScale = 0;
            //lifeManager.TakeLife();
            Destroy(gameObject);
        }
    }

    public static void HurtPlayer(int minDmg, int maxDmg)
    {
        playerHealth -= Random.Range(minDmg, maxDmg);
    }

    public static int GetHealth()
    {
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }

        return playerHealth;
    }

    public static int GetMaxHealth()
    {
        return maxHealth;
    }

    public void FullHealth()
    {
        playerHealth = maxPlayerHealth;
    }
}
