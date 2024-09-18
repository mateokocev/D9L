using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Reference to your canvases
    public GameObject mainMenuCanvas;
    public GameObject settingsMenuCanvas;

    // Start by showing the Main Menu and hiding others
    void Start()
    {
        ShowMainMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ShowSettingsMenu()
    {
        // Activate the settings menu and deactivate the main menu
        mainMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(true);
    }

    public void ShowMainMenu()
    {
        // Activate the main menu and deactivate the settings menu
        mainMenuCanvas.SetActive(true);
        settingsMenuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}