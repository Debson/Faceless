using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpingPlatform : MonoBehaviour
{

    private Vector2 startPos;
    private Vector2 endPos;
    private float temp;
    private float time;

    private bool bump;
    private bool playerNotOnPlatform;

    private void LateUpdate()
    {
        if(playerNotOnPlatform && !bump)
        {
            BackToStartPos();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" && !bump)
        {
            startPos = transform.position;
            endPos = new Vector2(startPos.x, startPos.y - 5f);
            bump = true;
            time = 0f;
            StartCoroutine(PlatformBump(startPos, endPos));
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerNotOnPlatform = true;
        }
    }

    IEnumerator PlatformBump(Vector2 startPos, Vector2 endPos)
    {
        while (time < 0.2f)
        {
            transform.position = Vector2.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }
        bump = false;
    }

    private void BackToStartPos()
    {
            startPos = transform.position;
            endPos = new Vector2(startPos.x, startPos.y + 5f);
            bump = true;
            playerNotOnPlatform = false;
            time = 0f;
            StartCoroutine(PlatformBump(startPos, endPos));
    }
}
