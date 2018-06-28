using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class PlayerManager : MonoBehaviour
    {

        public static PlayerManager instance = null;
        public GameObject selectedObject;

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}