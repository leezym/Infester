using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dieMenu : MonoBehaviour
{
    GameObject dieCanvas, mainCanvas, winCanvas;
    public GameObject player;
    public AudioClip heart, die;

    private void Awake()
    {
        mainCanvas = GameObject.Find("mainMenu");
        dieCanvas = GameObject.Find("dieMenu");
        winCanvas = GameObject.Find("winMenu");
    }

    void Start()
    {
        dieCanvas.SetActive(false);
        winCanvas.SetActive(false);
    }
    
    void Update()
    {
    
    }

    public void MainMenuGame()
    {
        GameObject.Find("BackgroundAudioSource").GetComponent<AudioSource>().Play();
        GameObject.Find("MenuAudioSource").GetComponent<AudioSource>().Stop();
        GameObject.Find("EnviromentAudioSource").GetComponent<AudioSource>().Pause();
        Time.timeScale = 1;
        dieCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        player.SetActive(false);
        gameObject.GetComponent<hudMenu>().reset();
        SceneManager.LoadScene("menuScene");
    }
}
