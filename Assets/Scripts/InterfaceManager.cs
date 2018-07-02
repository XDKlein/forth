using UnityEngine;
using UnityEditor;

namespace forth
{
    public class InterfaceManager : MonoBehaviour
    {
        public static InterfaceManager instance = null;
        public GameObject systemInfo = null;
        private StarSystem selectedSystem = null;

        public StarSystem SelectedSystem
        {
            get
            {
                return selectedSystem;
            }

            set
            {
                selectedSystem = value;
            }
        }

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
        
        //move to InputManager, when it comes more outer methods to process.
        public void SystemClick(StarSystem system)
        {
            if (selectedSystem != system)
            {
                DeselectSystem();
                SelectSystem(system);
            }
        }

        public void BackgroundClick()
        {
            DeselectSystem();
        }

        public void DeselectSystem()
        {
            if (SelectedSystem != null)
            {
                Destroy(SelectedSystem.GameObject.GetComponent<Projector>());
                HideSystemInfo();
                SelectedSystem = null;
            }
        }

        public void SelectSystem(StarSystem system)
        {
            if (SelectedSystem?.GameObject != system.GameObject)
            {
                GraphicsManager.instance.HighlightStarSystem(system.GameObject);
                SelectedSystem = system;
                ShowSystemInfo(system);
            }
        }

        public void ShowSystemInfo(StarSystem system)
        {
            GameObject systemInfoWindow = Instantiate(systemInfo);
            systemInfoWindow.transform.SetParent(system.GameObject.transform);
            systemInfoWindow.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
        }

        public void HideSystemInfo()
        {
            Destroy(SelectedSystem.GameObject.transform.Find("SystemInfo(Clone)").gameObject);
        }


        //public ShowSystemInfoWindow-like-function; 
    }
}