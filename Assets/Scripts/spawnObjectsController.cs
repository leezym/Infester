using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObjectsController : MonoBehaviour
{
    GameObject[] cheese, trap;
    public int amountCheese, amountTrap;
    GameObject prefabCheese, prefabTrap;
    [HideInInspector]
    public Transform[] instantiatePoint;

    void Start()
    {
        instantiatePoint = GameObject.Find("pointsPatrol").GetComponentsInChildren<Transform>();
        prefabCheese = Resources.Load("Cheese") as GameObject;
        prefabTrap = Resources.Load("Trap") as GameObject;

        int ceilR = (int) Mathf.Ceil((instantiatePoint.Length - 1) / 2); 

        cheese = new GameObject[amountCheese];
        for (int i = 0; i < amountCheese; i++)
        {
            Transform posInit = instantiatePoint[Random.Range(1, ceilR)];
            cheese[i] = Instantiate(prefabCheese, posInit.position, Quaternion.identity);
            cheese[i].transform.SetParent(GameObject.Find("itemsPool").GetComponent<Transform>());
        }

        trap = new GameObject[amountTrap];
        for (int i = 0; i < amountTrap; i++)
        {
            Transform posInit = instantiatePoint[Random.Range(ceilR+1, instantiatePoint.Length)];
            trap[i] = Instantiate(prefabTrap, posInit.position, Quaternion.identity);
            trap[i].transform.SetParent(GameObject.Find("itemsPool").GetComponent<Transform>());
        }
    }
    
    void Update()
    {
        
    }

    public void spawnNewItem(GameObject prefab)
    {
        Transform posInit = instantiatePoint[Random.Range(1, instantiatePoint.Length)]; // no se incluye el primero porque es el padre
        prefab.transform.position = posInit.transform.position;
        prefab.SetActive(true);
    }
}
