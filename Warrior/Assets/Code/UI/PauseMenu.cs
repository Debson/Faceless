using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;

    private bool gamePaused = false;

    protected void Start()
    {
        PauseUI.SetActive(false);
    }

    protected void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            gamePaused = !gamePaused;
        }

        if(gamePaused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void Resume()
    {
        gamePaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

   
}
