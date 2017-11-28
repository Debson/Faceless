using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmMovement : MonoBehaviour
{
    [SerializeField]
    Transform arm;

    [SerializeField]
    Transform hand;

    [SerializeField]
    Transform arrow;

    ElfController elfController;
    AudioManager audioManager;

    public bool startStretch { get; set; }

    protected void Awake()
    {
        elfController = GetComponent<ElfController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    protected void Start()
    {
        startStretch = true;
    }

    protected void Update()
    {

    }

    public IEnumerator MoveHandBeforeShoot()
    {
        if(startStretch)
        { 
            for (int i = 0; i < 55; i++)
            {
                arm.transform.rotation *= Quaternion.Euler(0, 0, arm.transform.position.z + 0.08f * i);
                hand.transform.position = new Vector2(hand.transform.position.x + ( 0.01f * (float)i / 47 * elfController.direction), hand.transform.position.y + 0.0011f * (float)i / 4);
               
                yield return 0;
            }
            audioManager.bow.Play();

            for (int i = 0; i < 55; i++)
            {
                arm.transform.rotation *= Quaternion.Euler(0, 0, arm.transform.position.z - 0.08f * i);
                hand.transform.position = new Vector2(hand.transform.position.x - (0.01f * (float)i / 47 * elfController.direction), hand.transform.position.y - 0.0011f * (float)i / 4);

                yield return 0;
            }
        }
    }
}
