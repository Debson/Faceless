using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Environment sounds
    /// </summary>

    public AudioSource coinSound;
    public AudioSource healSound;

    /// <summary>
    /// Player Sounds
    /// </summary>

    public AudioSource[] attackSound;
    public AudioSource[] monsterHurt;
    public AudioSource[] playerJump;
    public AudioSource[] playerHurt;

    /// <summary>
    /// Dragon Sounds
    /// </summary>

    public AudioSource dragonFlapping;
    public AudioSource dragonRoar;
    public AudioSource dragonAttack1;
    public AudioSource dragonAttack2;
    public AudioSource dragonStep;
    public AudioSource[] dragonPain;

    /// <summary>
    /// Orc sounds
    /// </summary>

    public AudioSource[] orcPain1;
    public AudioSource[] orcPain2;
    public AudioSource[] OrcRoar;
    public AudioSource[] orcWeapon;
    public AudioSource[] orcAttack;
    public AudioSource[] orcDeath;
}
