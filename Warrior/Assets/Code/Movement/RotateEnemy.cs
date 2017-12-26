using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RotateEnemy : MonoBehaviour {

    [SerializeField]
    private GameObject healthBar;

    [SerializeField]
    private float rotateDelay = 2f;

    static readonly Quaternion flipRotation = Quaternion.Euler(0, 180, 0);

    PlayerController player;

    public bool stopRotate { get; set; }
    public bool rotationCompleted { get; set; }

    protected void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public bool isFacingRight { get; private set; }
    public bool isFlipped { get; private set; }

    protected void Start()
    {
        StartCoroutine(Rotate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        Debug.Log(rotationCompleted);
    }

    IEnumerator Rotate()
    {
        if (!stopRotate)
        {
            while (true)
            {
                rotationCompleted = false;
                yield return new WaitForSeconds(rotateDelay);
                rotationCompleted = true;
                float xPosition = player.transform.position.x;

                if (xPosition > transform.position.x)
                {
                    bool isTravelingRight = xPosition > transform.position.x;
                    if (isFacingRight != isTravelingRight)
                    {
                        isFacingRight = isTravelingRight;
                        transform.rotation *= flipRotation;
                        healthBar.transform.rotation *= flipRotation;
                        isFlipped = true;
                    }
                }

                if (xPosition < transform.position.x && isFlipped)
                {
                    bool isTravelingRight = xPosition > transform.position.x;
                    if (isFacingRight != isTravelingRight)
                    {
                        isFacingRight = isTravelingRight;
                        transform.rotation *= flipRotation;
                        healthBar.transform.rotation *= flipRotation;
                        isFlipped = false;
                    }
                }
                yield return 0;
            }
        }
    }

}
