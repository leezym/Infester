using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using controller;

public class mainMenu : MonoBehaviour
{
    GameObject mainCanvas, hudCanvas;
    GameObject loadCanvas;
    AudioSource audioSourceTheme, audioSourceEnviroment;
    public Toggle checkMusic, checkMusicPause;
    pauseMenu pauseMenuScript;
    public Button loadGame;
    public GameObject player, initPlayer;

    private void Awake()
    {
        mainCanvas = GameObject.Find("mainMenu");
        hudCanvas = GameObject.Find("HUD");
        loadCanvas = GameObject.Find("load");
        audioSourceTheme = GameObject.Find("BackgroundAudioSource").GetComponent<AudioSource>();
        audioSourceEnviroment = GameObject.Find("EnviromentAudioSource").GetComponent<AudioSource>();
        pauseMenuScript = GameObject.FindObjectOfType<pauseMenu>().GetComponent<pauseMenu>();
        loadGame.gameObject.SetActive(false);
    }

    void Start()
    {
        mainCanvas.SetActive(true);
        loadCanvas.SetActive(false);
        GameObject.Find("EffectsAudioSource").GetComponent<AudioSource>().Stop();
    }

    void Update()
    {
        if (testController.data.saved)
        {
            loadGame.gameObject.SetActive(true);
        }
        else
        {
            loadGame.gameObject.SetActive(false);
        }
    }

    public void PlayGame()
    {
        mainCanvas.SetActive(false);
        loadCanvas.SetActive(true);
        testController.data.loader = true;

        // reset
        gameObject.GetComponent<hudMenu>().reset();
        testController.data.saved = false;
        // Posicion tentativa inicial del player new Vector3(-0.01,-2.086,-44.13) -> initPlayer
        player.transform.position = initPlayer.transform.position;
        
        loader.LoadLevel("mainScene");
        //SceneManager.LoadScene("mainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        mainCanvas.SetActive(false);
        loadCanvas.SetActive(true);
        testController.data.loader = true;

        loader.LoadLevel("mainScene");
        //SceneManager.LoadScene("mainScene");
    }

    public void MusicEnabled()
    {
        if (!checkMusic.isOn)
        {
            audioSourceTheme.mute = true;
            checkMusicPause.isOn = false;
        }
        else if (checkMusic.isOn)
        {
            audioSourceTheme.mute = false;
            checkMusicPause.isOn = true;
        }
    }
}
