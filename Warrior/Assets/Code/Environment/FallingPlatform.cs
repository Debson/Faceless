using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    public float fallDelay;

    private Rigidbody2D myBody;
    private Collider2D myCollider;
    private SpriteRenderer spriteRenderer;
    private Shader startShader;

    private float time;
    private float rotation = 0.1f;

    protected void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startShader = spriteRenderer.material.shader;
        time = 0f;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(0.1f);
        while(time < 0.2f)
        {
            var function = Random.value <= .5f ? (spriteRenderer.material.shader = Shader.Find("PaintWhite")) : (spriteRenderer.material.shader = startShader);
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }
        spriteRenderer.material.shader = startShader;
        myBody.isKinematic = false;
        myCollider.isTrigger = true;
        Destroy(gameObject, 2f);

        yield return 0;
    }

}
