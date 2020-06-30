using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    Rigidbody rb;
    playerController playerControllerScript;
    //public float bulletSpeed;

    void Start()
    {
    }
    
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject,5f);
    }
}
