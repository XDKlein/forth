using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance = null;

        public CameraController camera = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            camera = Camera.main.GetComponent<CameraController>();
            DontDestroyOnLoad(gameObject);
        }

        private void LateUpdate()
        {

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                camera.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                camera.Scroll(Input.GetAxis("Mouse ScrollWheel"));
            }

        }
    }
}