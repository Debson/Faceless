using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class OnTriggerLoadLevel : MonoBehaviour
{
    [SerializeField]
    private float loadDelay = 3f;

    [SerializeField]
    private string level;

    [SerializeField]
    private Image blackout;

    [SerializeField]
    private bool offSpriteRenderer;

    PlayerController playerController;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(LoadLevel(loadDelay));
        }
    }

    IEnumerator LoadLevel(float delay)
    {
        playerController.CharacterControlDisabled = true;
        if(offSpriteRenderer)
        {
            playerController.GetComponent<SpriteRenderer>().enabled = false;
        }
        while (blackout.color.a <= 1f)
        {
            blackout.color = new Color(0, 0, 0, blackout.color.a + Time.deltaTime);
            yield return 0;
        }
        yield return new WaitForSeconds(delay);
        EditorSceneManager.LoadScene(level);
    }
}

