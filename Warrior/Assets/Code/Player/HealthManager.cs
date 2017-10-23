using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxPlayerHealth;

    public static int playerHealth;

    LifeManager lifeManager;

    protected void Awake()
    {
        lifeManager = GetComponent<LifeManager>();
    }

    protected void Start()
    {
        playerHealth = maxPlayerHealth;
    }

    protected void Update()
    {
        //Debug.Log(playerHealth);
        if(playerHealth <= 0)
        {
            //lifeManager.TakeLife();
            Destroy(gameObject);
        }
    }

    public static void HurtPlayer(int damageToGive)
    {
        playerHealth -= damageToGive;
    }

    public static int GetHealth()
    {
        return playerHealth;
    }

    public void FullHealth()
    {
        playerHealth = maxPlayerHealth;
    }
}
