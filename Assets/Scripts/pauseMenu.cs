using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using controller;

public class pauseMenu : MonoBehaviour
{
    public GameObject player;
    public Toggle checkMusicPause, checkMusic;
    GameObject pauseCanvas, hudCanvas, mainCanvas;
    AudioSource audioSourceTheme, audioSourceEnviroment;
    mainMenu mainMenuScript;
    hudMenu hudMenuScript;

    private void Awake()
    {
        pauseCanvas = GameObject.Find("pauseMenu");
        hudCanvas = GameObject.Find("HUD");
        //checkMusicPause = pauseCanvas.GetComponent<Toggle>();
        audioSourceTheme = GameObject.Find("BackgroundAudioSource").GetComponent<AudioSource>();
        audioSourceEnviroment = GameObject.Find("EnviromentAudioSource").GetComponent<AudioSource>();
        mainCanvas = GameObject.Find("mainMenu");
        mainMenuScript = GameObject.FindObjectOfType<mainMenu>().GetComponent<mainMenu>();
        hudMenuScript = GameObject.FindObjectOfType<hudMenu>().GetComponent<hudMenu>();
    }

    void Start()
    {
        pauseCanvas.SetActive(false);
    }

    void Update()
    {
    }

    public void ContinueGame()
    {
        pauseCanvas.SetActive(false);
        hudCanvas.SetActive(true);
        Time.timeScale = 1;
        audioSourceEnviroment.mute = false;
    }

    public void MainMenuGame()
    {
        GameObject.Find("BackgroundAudioSource").GetComponent<AudioSource>().Play();
        GameObject.Find("MenuAudioSource").GetComponent<AudioSource>().Stop();
        GameObject.Find("EnviromentAudioSource").GetComponent<AudioSource>().Pause();
        pauseCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        Time.timeScale = 1;
        player.SetActive(false);
        SceneManager.LoadScene("menuScene");
    }

    public void SaveGame()
    {
        testController.data.saved = true;
    }

    public void MusicEnabled()
    {        
        if (!checkMusicPause.isOn)
        {
            audioSourceTheme.mute = true;
            checkMusic.isOn = false;
        }
        else if (checkMusicPause.isOn)
        {
            audioSourceTheme.mute = false;
            checkMusic.isOn = true;
        }
    }
}
