using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused;
    // Start is called before the first frame update

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame() {
        Application.Quit();
    }
    
}
