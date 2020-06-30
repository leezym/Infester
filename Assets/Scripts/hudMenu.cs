using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hudMenu : MonoBehaviour
{
    public Slider sliderLifeEnemy, sliderLifePlayer, sliderDamage;
    GameObject hudCanvas, dieCanvas, winCanvas;
    [HideInInspector]
    public int countLife = 3;
    [HideInInspector]
    public Image[] lifes;
    bool soundDie = true;

    private void Awake()
    {      
        hudCanvas = GameObject.Find("HUD");
        dieCanvas = GameObject.Find("dieMenu");
        winCanvas = GameObject.Find("winMenu");
        lifes = GameObject.Find("HUD/Rat/Lifes").GetComponentsInChildren<Image>();
    }

    void Start()
    {
        hudCanvas.SetActive(false);
    }

    void Update()
    {
        if (countLife == 0)
        {
            Time.timeScale = 0;
            dieCanvas.SetActive(true);
            hudCanvas.SetActive(false);
            GameObject.Find("EffectsAudioSource").GetComponent<AudioSource>().Stop();
            GameObject.Find("BackgroundAudioSource").GetComponent<AudioSource>().Stop();
            if (!GameObject.Find("MenuAudioSource").GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Find("MenuAudioSource").GetComponent<AudioSource>().PlayOneShot(GetComponent<dieMenu>().die, 0.2f);
                GameObject.Find("MenuAudioSource").GetComponent<AudioSource>().PlayOneShot(GetComponent<dieMenu>().heart);
            }
        }
        else
        {
            dieCanvas.SetActive(false);
        }

        if (sliderDamage.value >= sliderDamage.maxValue)
        {
            Time.timeScale = 0;
            winCanvas.SetActive(true);
            hudCanvas.SetActive(false);
            GameObject.Find("EffectsAudioSource").GetComponent<AudioSource>().Stop();            
        }
        else
        {
            winCanvas.SetActive(false);
        }
    }

    public void reset()
    {
        countLife = 3;
        sliderLifePlayer.value = sliderLifePlayer.maxValue;
        sliderDamage.value = sliderLifePlayer.minValue;
        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].enabled = true;
        }
    }
}
