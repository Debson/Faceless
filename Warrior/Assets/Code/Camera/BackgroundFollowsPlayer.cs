using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollowsPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject background;

    PlayerController player;

    protected void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    protected void LateUpdate ()
    {
        background.transform.position = player.transform.position;
	}
}
