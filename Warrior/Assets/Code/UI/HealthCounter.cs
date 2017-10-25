using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{

    private int playerHealth;

    Text text;
    LifeManager lifeManager;
    HealthManager healthManager;

    protected void Awake()
    {
        text = GetComponent<Text>();
        lifeManager = GetComponent<LifeManager>();
    }

    protected void Update()
    {
        text.text = "" + HealthManager.GetHealth() + "/" + HealthManager.GetMaxHealth();
    }
}
