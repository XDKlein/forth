﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace forth
{
    public class GraphicsManager : MonoBehaviour {

        public static GraphicsManager instance = null;

        [Header("Background Graphics")]
        public float parralax = 2f;
        public GameObject mainLayer;
        public GameObject parallaxLayer;
        public GameObject MainMapGuide;

        [Header("Stars and Connections")]
        public GameObject connectionLine;


        [Header("Interface Graphics")]
        public Material highlightMaterial;
        public GameObject systemTitle;
        public GameObject constellationTitle;
        public GameObject constellationBorder;

        public bool drawGizmo = false;
        public GameObject borderGizmo;

        private List<GameObject> connections = new List<GameObject>();
        private List<GameObject> systems = new List<GameObject>();
        private List<GameObject> guides = new List<GameObject>();

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            SetupBackground();
            SetupSolarSystems();
            SetupSystemsConnections();
            SetupConstellations();

            DrawMapGuides();
        }

        void Update()
        {
            //ProcessBackground();
        }

        public void RescaleMapElements()
        {
            foreach (GameObject connection in connections)
            {
                connection.GetComponent<LineRenderer>().widthMultiplier = 0.01f * Camera.main.orthographicSize;
            }
            foreach (GameObject guide in guides)
            {
                guide.GetComponent<LineRenderer>().widthMultiplier = 0.0075f * Camera.main.orthographicSize;
            }
        }

        //TODO: refactore
        void DrawMapGuides()
        {
            float sizeX = GameManager.instance.map.map.size.x - 2;
            float sizeY = GameManager.instance.map.map.size.y - 2;
            GameObject guidesParent = new GameObject("Guides");

            GameObject upperGuide = Instantiate(MainMapGuide);
            upperGuide.transform.SetParent(guidesParent.transform);
            upperGuide.GetComponent<LineRenderer>().SetPosition(0, new Vector3((-sizeX / 2) - 1f, sizeY / 2));
            upperGuide.GetComponent<LineRenderer>().SetPosition(1, new Vector3((sizeX / 2) + 1f, sizeY / 2));
            guides.Add(upperGuide);

            GameObject downGuide = Instantiate(MainMapGuide);
            downGuide.transform.SetParent(guidesParent.transform);
            downGuide.GetComponent<LineRenderer>().SetPosition(0, new Vector3((-sizeX / 2) - 1f, -sizeY / 2));
            downGuide.GetComponent<LineRenderer>().SetPosition(1, new Vector3((sizeX / 2) + 1f, -sizeY / 2));
            guides.Add(downGuide);

            GameObject leftGuide = Instantiate(MainMapGuide);
            leftGuide.transform.SetParent(guidesParent.transform);
            leftGuide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(-sizeX / 2, (sizeY / 2) + 1f ));
            leftGuide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(-sizeX / 2, -sizeY / 2 - 1f ));
            guides.Add(leftGuide);

            GameObject rightGuide = Instantiate(MainMapGuide);
            rightGuide.transform.SetParent(guidesParent.transform);
            rightGuide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(sizeX / 2, (-sizeY / 2) - 1f));
            rightGuide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(sizeX / 2, (sizeY / 2) + 1f));
            guides.Add(rightGuide);

            float indent = sizeX / 10;
            while(indent < sizeX)
            {
                GameObject secondaryGuide = Instantiate(MainMapGuide);
                secondaryGuide.transform.SetParent(guidesParent.transform);
                secondaryGuide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(-sizeX / 2 + indent, (sizeY / 2) + 0.2f));
                secondaryGuide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(-sizeX / 2 + indent, (-sizeY / 2) - 0.2f));
                guides.Add(secondaryGuide);
                indent += sizeX / 10;
            }

            indent = sizeY / 10;
            while (indent < sizeY)
            {
                GameObject secondaryGuide = Instantiate(MainMapGuide);
                secondaryGuide.transform.SetParent(guidesParent.transform);
                secondaryGuide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(-sizeX / 2 - 0.2f, (sizeY / 2) - indent));
                secondaryGuide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(sizeX / 2 - 0.2f, (sizeY / 2) - indent));
                guides.Add(secondaryGuide);
                indent += sizeY / 10;
            }
        }

        void SetupSolarSystems()
        {
            GameObject solParent = new GameObject("Systems");
            foreach (StarSystem sol in GameManager.instance.map.StarSystems)
            {
                GameObject systemGameObject = sol.GameObject;//Instantiate(sol.GameObject);
                systemGameObject.name = sol.Name;
                systemGameObject.AddComponent<ObjectData>().StoredData = sol;
                
                /*GameObject systemTitle = Instantiate(instance.systemTitle);
                systemTitle.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = sol.Name;
                systemTitle.transform.SetParent(systemGameObject.transform);*/

                systemGameObject.transform.SetParent(solParent.transform);
                Vector2 systemPosition = sol.Position;
                systemGameObject.transform.position = new Vector3(systemPosition.x - Random.Range(-0.5f, 0.5f),
                                                                  systemPosition.y - Random.Range(-0.5f, 0.5f),
                                                                  -1);
                float scale = Random.Range(0, 0.5f);
                //systemGameObject.transform.localScale = new Vector3(1 - scale, 1 - scale, 1);
                sol.GameObject = systemGameObject;
                systems.Add(systemGameObject);
            }
        }

        void SetupSystemsConnections()
        {
            GameObject connectionsParent = new GameObject("Connections");
            foreach (StarSystemConnection connection in GameManager.instance.map.StarSystemConnections)
            {
                GameObject connectionGameObject = Instantiate(connectionLine);

                connectionGameObject.transform.SetParent(connectionsParent.transform);
                StarSystem[] connectedSystems = connection.StarSystems;

                //Setting start and end point of line.
                connectionGameObject.GetComponent<LineRenderer>().SetPosition(0, new Vector3(
                    connectedSystems[0].GameObject.transform.position.x, connectedSystems[0].GameObject.transform.position.y));
                connectionGameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(
                    connectedSystems[1].GameObject.transform.position.x, connectedSystems[1].GameObject.transform.position.y));
                //

                connection.GameObject = connectionGameObject;
                connections.Add(connectionGameObject);
            }
        }

        void SetupConstellations()
        {
            foreach(Constellation constellation in GameManager.instance.map.Constellations)
            {
                //GameObject systemTitle = Instantiate(constellationTitle);
                //systemTitle.transform.GetChild(0).GetComponent<UnityEditor.TextM>().text = constellation.Name;
                //systemTitle.transform.GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, constellation.GetAngle());
                //systemTitle.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(constellation.GetWidth() / 2, constellation.GetHeight() / 2);
                //systemTitle.transform.position = constellation.Centroid;
            }

            foreach (Constellation constellation in GameManager.instance.map.Constellations)
            {
                /*int vsizeX = 1;
                int vsizeY = 1;

                List<Vector2> vertices2D = new List<Vector2>();
                foreach (Vector2 borderPoint in constellation.Border.Points.Keys)
                {
                    if (vertices2D.Find(x => x.x == borderPoint.x) == Vector2.zero)
                        vsizeX++;
                    if (vertices2D.Find(x => x.y == borderPoint.y) == Vector2.zero)
                        vsizeY++;
                    vertices2D.Add(borderPoint);
                }

                GameObject overlay = Instantiate(constellationOverlay);
                overlay.transform.position = Vector2.zero;

                Triangulator tr = new Triangulator(vertices2D.ToArray());
                int[] indices = tr.Triangulate();

                Vector3[] vertices = new Vector3[vertices2D.Count];
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
                }

                Mesh msh = new Mesh();
                msh.vertices = vertices;
                msh.triangles = indices;

                msh.RecalculateNormals();   
                msh.RecalculateBounds();

                Bounds bounds = msh.bounds;
                Vector2[] uvs = new Vector2[vertices.Length];
                for (int i = 0; i < vertices.Length; i++)
                {
                    uvs[i] = new Vector2(vertices[i].x / bounds.size.x, vertices[i].y / bounds.size.x);
                }
                msh.uv = uvs;

                MeshFilter filter = overlay.GetComponent<MeshFilter>();
                filter.mesh = msh;*/

                if (drawGizmo)
                {
                    foreach (Vector2 borderPoint in constellation.Border.Points.Keys)
                    {
                        GameObject gizmo = Instantiate(borderGizmo);
                        gizmo.transform.position = borderPoint;
                        gizmo.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    }
                }

                GameObject borderLine = Instantiate(constellationBorder);
                borderLine.GetComponent<LineRenderer>().positionCount = constellation.Border.Points.Count;
                int index = 0;
                foreach (Vector2 borderPoint in constellation.Border.Points.Keys)
                {
                    borderLine.GetComponent<LineRenderer>().SetPosition(index, borderPoint);
                    index++;
                }
                //break;
            }
        }

        public void HighlightStarSystem(GameObject system)
        {
            Projector prj = system.AddComponent<Projector>();
            prj.orthographic = true;
            prj.orthographicSize = 1;
            prj.material = highlightMaterial;
        }


        void SetupBackground()
        {
            SetupBackgroundMainLayer(GameManager.instance.map.map.size);
            SetupBackgroundParallaxLayer();
        }

        public void ProcessBackground()
        {
            Material background = parallaxLayer.GetComponent<MeshRenderer>().material;
            Vector2 offset = background.mainTextureOffset;

            offset.x = parallaxLayer.transform.position.x / parallaxLayer.transform.localScale.x / parralax;
            offset.y = parallaxLayer.transform.position.y / parallaxLayer.transform.localScale.y / parralax;

            background.mainTextureOffset = offset;

            parallaxLayer.GetComponent<MeshRenderer>().material = background;
        }

        ///<summary>
        ///Rescale background layer relate to camera;
        ///</summary>
        public void RescaleBackground()
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
        public void SetupBackgroundMainLayer(Vector2 scale)
        {
            mainLayer = Instantiate(mainLayer);
            mainLayer.AddComponent<ObjectData>().StoredData = new MapBackground();

            mainLayer.transform.position = new Vector3(0, 0, 0);
            mainLayer.transform.localScale = new Vector3(scale.x, scale.y, 0);

            //float tileSize = Mathf.Clamp(scale.x, 1f, 4f);
            //mainLayer.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(tileSize, tileSize);
        }

        ///<summary>
        ///Set position, scale and texture scale of background parallax layer;
        ///</summary>
        public void SetupBackgroundParallaxLayer()
        {
            parallaxLayer = Instantiate(parallaxLayer);

            parallaxLayer.transform.position = new Vector3(0, 0, 0);
            //parallaxLayer.transform.SetParent(Camera.main.transform);

            float quadHeight = Camera.main.orthographicSize * 2.0f;
            float quadWidth = quadHeight * Screen.width / Screen.height;
            parallaxLayer.transform.localScale = new Vector3(quadWidth, quadHeight, 1);
        }
    }
}