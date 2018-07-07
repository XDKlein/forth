using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class CameraController : MonoBehaviour {
        public float panSpeed = 5f;
        public float panBorderThickness = 10f;

        public float scrollSpeed = 5f;
        public float minZ = 10f;
        public float maxZ = 120f;

        public Vector2 panLimit;

        void Start()
        {
            panLimit = new Vector2(GameManager.instance.map.mapSize.x + 5, GameManager.instance.map.mapSize.y + 5);
            Camera.main.transform.position = new Vector3(0, 0, -10);
            this.gameObject.GetComponent<Camera>().orthographicSize = (minZ + maxZ) / 2;
            this.maxZ = (panLimit.x / 2) * Screen.height / Screen.width;
        }

        void LateUpdate()
        {
            
        }

        public void Move(float vertical, float horizontal)
        {
            Vector3 newPosition = transform.position;

            if (vertical != 0)
            {
                newPosition.y += vertical * Time.deltaTime * panSpeed;
            }
            if(horizontal != 0)
            {
                newPosition.x += horizontal * Time.deltaTime * panSpeed;
            }

            transform.position = SetIntoBorders(newPosition);
        }

        public void Scroll(float zoom)
        {
            float currentZoom = this.gameObject.GetComponent<Camera>().orthographicSize;
            float zoomExtent = (panLimit.x / 2) * Screen.height / Screen.width;
            float newZoom = currentZoom - zoom * scrollSpeed * Time.deltaTime;

            newZoom = Mathf.Clamp(newZoom, minZ, maxZ);

            if (newZoom >= zoomExtent)
                this.gameObject.GetComponent<Camera>().orthographicSize = zoomExtent;
            else
                this.gameObject.GetComponent<Camera>().orthographicSize = newZoom;

            transform.position = SetIntoBorders(transform.position);
        }

        public Vector3 SetIntoBorders(Vector3 newPosition)
        {
            float vertExtent = this.gameObject.GetComponent<Camera>().orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            newPosition.x = Mathf.Clamp(newPosition.x, -panLimit.x / 2 + horzExtent, panLimit.x / 2 - horzExtent);
            newPosition.y = Mathf.Clamp(newPosition.y, -panLimit.y / 2 + vertExtent, panLimit.y / 2 - vertExtent);

            return newPosition;
        }
    }
}