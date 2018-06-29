using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class GraphicsManager : MonoBehaviour {

        public static GraphicsManager instance = null;
        public GameObject mainLayer;
        public GameObject parallaxLayer;

        //TODO: Move to own classes
        public GameObject[] starSprites;
        public GameObject connectionLine;

        //TODO: Move to interface processing.
        public GameObject systemTitle;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Start() {
            SetupBackground();
            SetupSolarSystems();
            SetupSystemsConnections();
        }

        void Update() {

        }

        void SetupBackground()
        {
            mainLayer = Instantiate(mainLayer);
            parallaxLayer = Instantiate(parallaxLayer);

            mainLayer.GetComponent<BackgroundController>().SetupMainLayer(GameManager.instance.map.mapSize);
            parallaxLayer.GetComponent<BackgroundController>().SetupParallaxLayer();
        }

        //TODO: Move body to controller.
        void SetupSolarSystems()
        {
            GameObject solParent = new GameObject("Systems");
            foreach (StarSystem sol in GameManager.instance.map.StarSystems)
            {
                GameObject systemGameObject = Instantiate(GraphicsManager.instance.starSprites[0]);

                //TODO: Move to interface processing.
                GameObject systemTitle = Instantiate(GraphicsManager.instance.systemTitle);
                systemTitle.GetComponent<TextMesh>().text = sol.Name;
                systemTitle.transform.SetParent(systemGameObject.transform);
                //

                systemGameObject.transform.SetParent(solParent.transform);
                Vector2 systemPosition = sol.Position;
                systemGameObject.transform.position = new Vector3(systemPosition.x - Random.Range(-0.5f, 0.5f),
                                                                  systemPosition.y - Random.Range(-0.5f, 0.5f),
                                                                  -1);
                float scale = Random.Range(0, 0.5f);
                systemGameObject.transform.localScale = new Vector3(1 - scale, 1 - scale, 1);
                sol.GameObject = systemGameObject;
            }
        }

        void SetupSystemsConnections()
        {
            GameObject connectionsParent = new GameObject("Connections");
            foreach (StarSystemConnection connection in GameManager.instance.map.StarSystemConnections)
            {
                GameObject connectionGameObject = Instantiate(GraphicsManager.instance.connectionLine);

                connectionGameObject.transform.SetParent(connectionsParent.transform);
                StarSystem[] connectedSystems = connection.StarSystems;

                //Setting start and end point of line.
                connectionGameObject.GetComponent<LineRenderer>().SetPosition(0, new Vector3(
                    connectedSystems[0].GameObject.transform.position.x, connectedSystems[0].GameObject.transform.position.y));
                connectionGameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(
                    connectedSystems[1].GameObject.transform.position.x, connectedSystems[1].GameObject.transform.position.y));
                //

                connection.GameObject = connectionGameObject;
            }
        }
    }
}