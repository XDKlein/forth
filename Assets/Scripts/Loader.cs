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
        public GameObject interfaceManager;
        public GameObject soundManager;

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

            if (InterfaceManager.instance == null)
                Instantiate(interfaceManager);
        }
    }
}