using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public AudioClip mainMenuMusic;

    private AudioSource audioSource;


    void Start()
    {
        ShowMainMenu();
        audioSource = GetComponent<AudioSource>();
        PlayMainMenuMusic();
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
    }

    private void PlayMainMenuMusic()
    {
        if (audioSource && mainMenuMusic)
        {
            audioSource.clip = mainMenuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopMainMenuMusic()
    {
        if (audioSource)
        {
            audioSource.Stop();
        }
    }

    public void PlayGame()
    {
        StopMainMenuMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void QuitGame()
    {
        StopMainMenuMusic();
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}