using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class BackgroundController : MonoBehaviour
    {

        public float parralax = 2f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Material background = GetComponent<MeshRenderer>().material;
            Vector2 offset = background.mainTextureOffset;

            offset.x = transform.position.x / transform.localScale.x / parralax;
            offset.y = transform.position.y / transform.localScale.y / parralax;

            background.mainTextureOffset = offset;
        }

        private void OnMouseDown()
        {
            if (PlayerManager.instance.selectedObject != null)
            {
                PlayerManager.instance.selectedObject.GetComponent<StarSystemController>().DeselectSystem();
            }
        }


        ///<summary>
        ///Rescale background layer relate to camera;
        ///</summary>
        public void Rescale()
        {
            float quadHeight = Camera.main.orthographicSize * 2.0f;
            float quadWidth = quadHeight * Screen.width / Screen.height;
            transform.localScale = new Vector3(quadWidth, quadHeight, 1);

            GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(quadWidth / 10, quadWidth / 10);

            //Material background = GetComponent<MeshRenderer>().material;
            //Vector2 scale = background.mainTextureScale;

            //scale.x = transform.localScale.x / 10f;
            //scale.y = transform.localScale.y / 10f;

            //background.mainTextureScale = scale;
        }

        ///<summary>
        ///Set position, scale and texture scale of background main layer;
        ///</summary>
        public void SetupMainLayer(Vector2 scale)
        {
            transform.position = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(scale.x, scale.y, 0);

            float tileSize = Mathf.Clamp(scale.x, 1f, 4f);
            GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(tileSize, tileSize);
        }

        ///<summary>
        ///Set position, scale and texture scale of background parallax layer;
        ///</summary>
        public void SetupParallaxLayer()
        {
            transform.position = new Vector3(0, 0, 0);
            transform.SetParent(Camera.main.transform);

            float quadHeight = Camera.main.orthographicSize * 2.0f;
            float quadWidth = quadHeight * Screen.width / Screen.height;
            transform.localScale = new Vector3(quadWidth, quadHeight, 1);
        }
    }
}