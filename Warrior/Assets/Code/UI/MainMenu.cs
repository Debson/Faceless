using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string startLevel;

    public string levelSelect;

    public int playerLives;

    public void NewGame()
    {
        SceneManager.LoadScene(startLevel);

        //PlayerPrefs.SetInt("PlayerCurrentLives", playerLives);
    }

    public void LevelSelect()
    {
        //PlayerPrefs.SetInt("PlayerCurrentLives", playerLives);
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
	
}
