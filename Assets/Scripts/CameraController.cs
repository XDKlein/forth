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

        private float camSize;
        private Vector2 panLimit;
        void Start()
        {
            panLimit = GameManager.instance.map.mapSize;
            Camera.main.transform.position = new Vector3(0, 0, -10);
            camSize = (minZ + maxZ) / 2;
        }

        void LateUpdate()
        {
            Vector3 pos = transform.position;

            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += Time.deltaTime * panSpeed;
            }
            else if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= Time.deltaTime * panSpeed;
            }

            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.y += Time.deltaTime * panSpeed;
            }
            else if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.y -= Time.deltaTime * panSpeed;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                camSize -= scroll * scrollSpeed * 100f * Time.deltaTime;
                camSize = Mathf.Clamp(camSize, minZ, maxZ);
                camSize = Mathf.Round(camSize * 10f) / 10f;
            }

            Camera.main.orthographicSize = Mathf.Round(Camera.main.orthographicSize * 10f) / 10f;
            if (Camera.main.orthographicSize < camSize)
            {
                Camera.main.orthographicSize += 0.1f;
                //GraphicsManager.instance.parallaxLayer.GetComponent<BackgroundController>().Rescale();
            }
            else if (Camera.main.orthographicSize > camSize)
            {
                Camera.main.orthographicSize -= 0.1f;
                //GraphicsManager.instance.parallaxLayer.GetComponent<BackgroundController>().Rescale();
            }

            float vertExtent = Camera.main.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            pos.x = Mathf.Clamp(pos.x , -panLimit.x / 2 + horzExtent, panLimit.x / 2 - horzExtent);
            pos.y = Mathf.Clamp(pos.y, -panLimit.y / 2 + vertExtent, panLimit.y / 2 - vertExtent);
            transform.position = pos;
        }
    }
}