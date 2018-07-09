using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth {
    [Serializable]
    public class MapManager {
        public MapType map;

       /* public Vector2 mapSize = new Vector2(50f, 50f);
        public int minStarSystems = 20;
        public int maxStarSystems = 50;

        public Texture2D galaxyType;*/

        private List<StarSystem> starSystems = new List<StarSystem>();
        private List<StarSystemConnection> starSystemConnections = new List<StarSystemConnection>();

        public List<StarSystem> StarSystems
        {
            get
            {
                return starSystems;
            }

            set
            {
                starSystems = value;
            }
        }

        public List<StarSystemConnection> StarSystemConnections
        {
            get
            {
                return starSystemConnections;
            }

            set
            {
                starSystemConnections = value;
            }
        }

        ///<summary>
        ///This is a description of my function.
        ///</summary>
        public void GenerateMap()
        {
            PlaceStarSystems();
            ConnectStarSystems();
        }

        void PlaceStarSystems()
        {
            List<Vector2> positions = new List<Vector2>();
            int starSystemCount = (int)UnityEngine.Random.Range(map.minSystems, map.maxSystems);
            List<SystemTypeAndProbability> systemTypes = map.systemTypes;
            List<string> usedNames = new List<string>();

            for (int count = 0; count < starSystemCount;)
            {
                Vector2 position = new Vector2((int)UnityEngine.Random.Range((map.size.x / 2 - 2) * -1f, (map.size.x / 2 - 2)),
                                               (int)UnityEngine.Random.Range((map.size.y / 2 - 2) * -1f, (map.size.x / 2 - 2)));
                if (!isPositionSuitable(positions, position))
                    continue;

                if (UnityEngine.Random.Range(0f, 1f) > GetTemplateGreyscale(position.x, position.y))
                    continue;

                positions.Add(position);

                //TODO: refactore code below -> split into several functions or just move to it's own 
                int index = 0;
                StarSystem newSystem = null;
                while (newSystem == null)
                {
                    if (systemTypes.Count == 0)
                        break;
                    if (index >= systemTypes.Count)
                        index = 0;
                    if (systemTypes.Count == 1 || UnityEngine.Random.Range(0f, 1f) <= systemTypes[index].probability)
                    {
                        SystemType type = systemTypes[index].systemType;
                        string newName = null;
                        foreach (string name in type.names)
                        {
                            if(!usedNames.Contains(name))
                            {
                                newName = name;
                                break;
                            }
                        }
                        if (newName == null && !type.useDefault)
                        {
                            systemTypes.RemoveAt(index);
                            continue;
                        }
                        else if (newName == null && type.useDefault) //TODO: replace with default names
                            newName = "System";

                        int subindex = 0;
                        while (newSystem == null)
                        {
                            if (type.gameObjects.Count == 0)
                            {
                                systemTypes.RemoveAt(index);
                                break;
                            }
                            if (subindex >= type.gameObjects.Count)
                                subindex = 0;
                            if (type.gameObjects.Count == 1 || UnityEngine.Random.Range(0f, 1f) <= type.gameObjects[subindex].probability)
                            {
                                GameObject gameObject = GameObject.Instantiate(type.gameObjects[subindex].gameObject);
                                float size = UnityEngine.Random.Range(type.minSizeMultiplier, type.maxSizeMultiplier);
                                gameObject.transform.GetChild(0).localScale = new Vector3(size, 1, size);
                                newSystem = new StarSystem(newName, gameObject, position);
                            }
                            subindex++;
                        }
                    }
                    index++;
                }

                StarSystems.Add(newSystem);
                usedNames.Add(newSystem.Name);
                if (systemTypes.Count == 0)
                    break;    
                count++;
            }
        }

        float GetTemplateGreyscale(float x, float y)
        {
            float mapToTextureRatio = map.size.x / map.template.width;
            return map.template.GetPixel((int)((x + map.size.x / 2) / mapToTextureRatio), (int)((y + map.size.y / 2) / mapToTextureRatio)).grayscale;
        }

        void ConnectStarSystems()
        {
            foreach(StarSystem origin in StarSystems)
            {
                StarSystemConnection systemConnection = origin.ConnectToClosest(StarSystems);
                if (systemConnection != null)
                    StarSystemConnections.Add(systemConnection);
            }
            //connectBlindSystems();
        }

        void connectBlindSystems()
        {
            foreach(StarSystem blindSystem in StarSystems)
            {
                if(blindSystem.Connections.Count == 1)
                {
                    StarSystemConnection systemConnection = blindSystem.ConnectToClosest(StarSystems);
                    if (systemConnection != null)
                        StarSystemConnections.Add(systemConnection);
                }
            }
        }

        //IDEAS: ConnectionLength (vector2 difference) as alternatives choose parameter; Storing shortest lengths in every node to every node;
        List<StarSystem> GetPath(StarSystem from, StarSystem to, List<StarSystem> path)
        {
            path.Add(from);
            List<StarSystem> originNeighbours = from.GetConnectedNeighbours();

            foreach (StarSystem destination in originNeighbours)
            {
                if (path.IndexOf(destination) >= 0)
                    continue;
                if(destination == to)
                {
                    path.Add(to);
                    return path;
                }
                List<StarSystem> subresult = GetPath(destination, to, new List<StarSystem>(path));
                if(subresult != null)
                {
                    if (path.IndexOf(to) > 0)
                    {
                        if (subresult.Count < path.Count)
                        {
                            path = subresult;
                        }
                    }
                    else
                        path = subresult;
                }
            }
            if (path.IndexOf(to) > 0)
                return path;
            return null;
        }

        bool isPositionSuitable(List<Vector2> positions, Vector2 position)
        {
            foreach(Vector2 pos in positions)
            {
                if (pos.x == position.x || pos.x - 1f == position.x || pos.x + 1f == position.x || pos.x - 2f == position.x || pos.x + 2f == position.x)
                    if (pos.y == position.y || pos.y - 1f == position.y || pos.y + 1f == position.y || pos.y - 2f == position.y || pos.y + 2f == position.y)
                        return false;
            }
            return true;
        }
    }
}