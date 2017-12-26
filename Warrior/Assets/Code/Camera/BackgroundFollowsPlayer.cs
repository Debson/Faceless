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
        background.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, background.transform.position.z);
	}
}
