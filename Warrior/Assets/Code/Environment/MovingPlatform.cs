using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    public float speed = 0.05f;

    [SerializeField]
    Transform start;

    [SerializeField]
    Transform end;

    FloorDetector floorDetector;
    private Transform temp;

    private float time = 0f;

    private bool callOnce;
    private bool startCorioutineOnce;

    protected void Awake()
    {
        floorDetector = FindObjectOfType<FloorDetector>();
        transform.position = start.transform.position;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.parent = transform;

            if (!startCorioutineOnce)
            {
                StartCoroutine(Move());
                startCorioutineOnce = true;
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }

    IEnumerator Move()
    {
        while(true)
        {
            transform.position = Vector2.Lerp(start.transform.position, end.transform.position, time * speed);
            time += Time.fixedDeltaTime;
            if(transform.position.x  >= end.transform.position.x && !callOnce)
            {
                time = 0;
                temp = start;
                start = end;
                end = temp;
                callOnce = true;
                yield return new WaitForSeconds(2f);
            }

            if(transform.position.x <= end.transform.position.x && callOnce)
            {
                time = 0;
                temp = end;
                end = start;
                start = temp;
                callOnce = false;
                yield return new WaitForSeconds(2f);
            }
            yield return 0;
        }
    }
}
