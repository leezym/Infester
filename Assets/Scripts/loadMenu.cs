using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using controller;

public class loadMenu : MonoBehaviour
{

    IEnumerator makeTheLoad(string level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);
        while (operation.isDone == false)
        {
            yield return null;
        }
    }

    private void Update()
    {
        if (testController.data.loader)
        {
            string levelToLoad = loader.nextLevel;
            StartCoroutine(makeTheLoad(levelToLoad));
            testController.data.loader = false;
        }
    }
}
