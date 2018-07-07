using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance = null;

        private Camera camera = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            camera = Camera.main;
            DontDestroyOnLoad(gameObject);
        }

        private void LateUpdate()
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                camera.GetComponent<CameraController>().Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            }

            if (GetScroll() != 0)
            {
                camera.GetComponent<CameraController>().Scroll(GetScroll());
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider?.gameObject.GetComponent<ObjectData>() != null)
                {
                    GameEntity entity = hit.collider.gameObject.GetComponent<ObjectData>().StoredData;
                    if (entity.Is(typeof(StarSystem)))
                    {
                        StarSystem system = entity.ToStarSystem();
                        InterfaceManager.instance.SystemClick(system);
                    }  
                }
                else
                {
                    InterfaceManager.instance.BackgroundClick();
                }
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