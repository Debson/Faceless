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

    public AudioSource[] comboSound;
    public AudioSource[] attackSound;
    public AudioSource[] monsterHurt;
    public AudioSource[] playerJump;
    public AudioSource[] playerHurt;
    public AudioSource[] playerDash;
    public AudioSource playerDie;

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
    
    /// <summary>
    /// Elf sounds
    /// </summary>

    public AudioSource bow;
    public AudioSource bowArrow;
    public AudioSource[] elfHurt;
    public AudioSource[] elfDeath;
    public AudioSource[] elfAttack;


    /// <summary>
    /// Spider sounds
    /// </summary>
    public AudioSource spiderAttack;
    public AudioSource spiderChattering;


    /// <summary>
    /// Reapers sounds
    /// </summary>
    public AudioSource[] reaperAttack;
    public AudioSource[] reaperHurt;
    public AudioSource[] reaperAgro;
    public AudioSource[] reaperDead;

}
