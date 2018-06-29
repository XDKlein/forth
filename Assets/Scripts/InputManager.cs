using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance = null;

        private CameraController camera = null;

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

            if (GetScroll() != 0)
            {
                camera.Scroll(GetScroll());
            }
        }

        private float scrollValue = 0f;
        private float GetScroll()
        {
            float sensitivity = 3f;
            float dead = 0.001f;

            float target = Input.GetAxisRaw("Mouse ScrollWheel");
            scrollValue = Mathf.MoveTowards(scrollValue,
                          target, sensitivity * Time.deltaTime);
            return (Mathf.Abs(scrollValue) < dead) ? 0f : scrollValue;
        }
    }
}