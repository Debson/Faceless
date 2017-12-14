using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private int maxPlayerHealth;

    public delegate void OnPlayerHurtKnockback(GameObject enemy);
    public static event OnPlayerHurtKnockback onPlayerHurtKnockback;

    private static int maxHealth;
    private static int playerHealth;

    LifeManager lifeManager;
    AudioManager audioManager;
    WalkMovement walkMovement;

    protected void Awake()
    {
        lifeManager = GetComponent<LifeManager>();
        audioManager = FindObjectOfType<AudioManager>();
        walkMovement = FindObjectOfType<WalkMovement>();
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
            audioManager.playerDie.Play();
            Destroy(this.gameObject);
        }
    }

    public void AttackPlayer(GameObject enemy, int minDamageToGive, int maxDamageToGive)
    {
        HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
        HurtPlayerOnContact.onAttackDamage -= AttackPlayer;

        onPlayerHurtKnockback += walkMovement.Knockback;
        if (onPlayerHurtKnockback != null)
        {
            onPlayerHurtKnockback(enemy);
        }
        //audioManager.playerHurt[Random.Range(0, 2)].Play();
    }

    public static void AddHealth(int health)
    {
        playerHealth += health;
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
