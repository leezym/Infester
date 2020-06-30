using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemyController : MonoBehaviour
{
    GameObject[] enemys;
    public int amountEnemys;
    GameObject prefabHuman;
    [HideInInspector]
    public Transform[] instantiatePoint;

    void Start()
    {
        instantiatePoint = GameObject.Find("pointsPatrol").GetComponentsInChildren<Transform>();
        prefabHuman = Resources.Load("Human") as GameObject;

        enemys = new GameObject[amountEnemys];
        for (int i = 0; i < amountEnemys; i++)
        {
            Transform posInit = instantiatePoint[Random.Range(1, instantiatePoint.Length)]; // no se incluye el primero porque es el padre
            enemys[i] = Instantiate(prefabHuman, posInit.localPosition, Quaternion.identity);
            enemys[i].transform.SetParent(GameObject.Find("enemysPool").GetComponent<Transform>());             
        }
    }
    
    void Update()
    {

    }

    public void spawnNewEnemy(GameObject prefabHuman)
    {
        Transform posInit = instantiatePoint[Random.Range(1, instantiatePoint.Length)]; // no se incluye el primero porque es el padre
        prefabHuman.transform.position = posInit.localPosition;
        prefabHuman.GetComponent<enemyController>().die = false;
        prefabHuman.GetComponent<enemyController>().walk = true;
        prefabHuman.GetComponent<enemyController>().fight = false;
        prefabHuman.GetComponent<enemyController>().throwB = false;
        prefabHuman.GetComponent<enemyController>().lifeEnemy = 100;
        prefabHuman.SetActive(true);
    }
}
