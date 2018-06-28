using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class Loader : MonoBehaviour
    {

        public GameObject gameManager;          //GameManager prefab to instantiate.
        public GameObject graphicsManager;      //GraphicsManager prefab to instantiate.
        public GameObject playerManager;      //GraphicsManager prefab to instantiate.
        public GameObject soundManager;         //SoundManager prefab to instantiate.

        public Font font;         //SoundManager prefab to instantiate.

        void Awake()
        {
            //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
            if (GameManager.instance == null)
                Instantiate(gameManager);

            if (GraphicsManager.instance == null)
                Instantiate(graphicsManager);

            if (PlayerManager.instance == null)
                Instantiate(playerManager);

            font.material.mainTexture.filterMode = FilterMode.Point;

            ////Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
            //if (SoundManager.instance == null)

            //    //Instantiate SoundManager prefab
            //    Instantiate(soundManager);
        }
    }
}