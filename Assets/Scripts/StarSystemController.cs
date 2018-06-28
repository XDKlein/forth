using UnityEngine;

namespace forth
{
    public class StarSystemController : MonoBehaviour
    {
        public Material highlightMaterial;
        

        // Use this for initialization
        void Start()
        {

        }

        private void OnMouseDown()
        {
            if (PlayerManager.instance.selectedObject != gameObject)
            {
                DeselectSystem();
                SelectSystem();
            }
        }

        public void DeselectSystem()
        {
            if (PlayerManager.instance.selectedObject != null)
            {
                Destroy(PlayerManager.instance.selectedObject.GetComponent<Projector>());
                PlayerManager.instance.selectedObject = null;
            }
        }

        public void SelectSystem()
        {
            Projector prj = gameObject.AddComponent<Projector>();
            prj.orthographic = true;
            prj.orthographicSize = 1;
            prj.material = highlightMaterial;
            PlayerManager.instance.selectedObject = gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}