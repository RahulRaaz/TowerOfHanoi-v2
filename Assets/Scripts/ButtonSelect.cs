using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSelect : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene(1); //Start game
    }
    public void quitGame()
    {
        Application.Quit(); //close app
    }
    public void pauseGame()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1; //Pause and resume app
        else if (Time.timeScale == 1)
            Time.timeScale = 0;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(0); //quit current game and go to main menu
    }

    public void playVideo()
    {
        SceneManager.LoadScene(3); //Play the video
    }
}
