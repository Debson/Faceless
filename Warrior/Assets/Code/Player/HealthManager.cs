using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxPlayerHealth;

    public static int playerHealth;

    Text text;
    LifeManager lifeManager;

    protected void Awake()
    {
        text = GetComponent<Text>();
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
            lifeManager.TakeLife();
            Destroy(gameObject);
        }

        text.text = "" + playerHealth;
    }

    public static void HurtPlayer(int damageToGive)
    {
        playerHealth -= damageToGive;
    }

    public void FullHealth()
    {
        playerHealth = maxPlayerHealth;
    }
}
