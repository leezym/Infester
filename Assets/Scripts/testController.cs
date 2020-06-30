using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace controller
{
    public class testController : MonoBehaviour
    {
        public static testController data;
        public bool saved = false;
        public bool loader = false;

        void Awake()
        {
            if (data == null)
            {
                DontDestroyOnLoad(gameObject);
                data = this;
            }
            else if (data != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
