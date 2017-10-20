using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    Text text;

    protected void Start()
    {
        text = GetComponent<Text>();

        score = 0;
    }

    protected void Update()
    {
        if(score < 0)
        {
            score = 0;
        }

        text.text = "" + score;
    }

    public static void AddPoints(int pointsToAdd)
    {
        score += pointsToAdd;
    }

    public static void Reset()
    {
        score = 0;
    }
}
