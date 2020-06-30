using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using controller;

public class startGame : MonoBehaviour
{
    //public Canvas hudCanvas, loadCanvas;

    void Awake()
    {
        //Time.timeScale = 1;
        testController.data.loader = false;
        GameObject.FindObjectOfType<mainMenu>().player.SetActive(true);
        GameObject.Find("GameController/Menus/HUD").SetActive(true);
        GameObject.Find("GameController/Menus/load").SetActive(false);
        //GameObject.Find("EnviromentAudioSource").GetComponent<AudioSource>().mute = false;
        GameObject.Find("EffectsAudioSource").GetComponent<AudioSource>().Play();
        GameObject.Find("EnviromentAudioSource").GetComponent<AudioSource>().Play();
    }
}
