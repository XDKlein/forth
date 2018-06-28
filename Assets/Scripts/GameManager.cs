using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        public MapManager map;

        void Awake()
        { 
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            map.GenerateMap();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}