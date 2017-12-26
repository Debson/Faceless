using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitialization : MonoBehaviour
{
    [SerializeField]
    private float loadDelay = 3f;

    [SerializeField]
    private Image blackout;

    PlayerController playerController;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        blackout.color = new Color(0, 0, 0, 1);
    }

    protected void Start()
    {
        StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(1f);
        while (blackout.color.a >= 0f)
        {
            blackout.color = new Color(0, 0, 0, blackout.color.a - Time.deltaTime);
            yield return 0;
        }
        playerController.CharacterControlDisabled = false;
    }
}
