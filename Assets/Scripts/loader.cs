using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using controller;

public class loader : MonoBehaviour
{
    public static string nextLevel;
    public static void LoadLevel(string name)
    {
        nextLevel = name;
        //testController.data.loader = true;
        //SceneManager.LoadScene("loadScene");
        
    }
}
