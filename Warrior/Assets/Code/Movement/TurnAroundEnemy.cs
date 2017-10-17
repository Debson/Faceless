using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TurnAroundEnemy : MonoBehaviour {

    [SerializeField]
    GameObject gameObjectWhichEnemyFollows;

    static readonly Quaternion flipRotation = Quaternion.Euler(0, 180, 0);

    public bool isFacingRight
    {
        get; private set;
    }

    public bool isFlipped;


    
	void Update ()
    {
        float xPosition = gameObjectWhichEnemyFollows.transform.position.x;

        if (xPosition > transform.position.x)
        {
            bool isTravelingRight = xPosition > transform.position.x;
            if (isFacingRight != isTravelingRight)
            {
                isFacingRight = isTravelingRight;
                transform.rotation *= flipRotation;
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
                isFlipped = false;
            }
        }
    }
}
