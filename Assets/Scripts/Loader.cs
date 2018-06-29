using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class Loader : MonoBehaviour
    {
        public GameObject inputManager;
        public GameObject gameManager;
        public GameObject graphicsManager;
        public GameObject playerManager;
        public GameObject soundManager;

        public Font font;

        void Awake()
        {
            if (InputManager.instance == null)
                Instantiate(inputManager);

            if (GameManager.instance == null)
                Instantiate(gameManager);

            if (GraphicsManager.instance == null)
                Instantiate(graphicsManager);

            if (PlayerManager.instance == null)
                Instantiate(playerManager);

            font.material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}