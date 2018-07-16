using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth {
    [Serializable]
    public class MapManager {
        public MapType map;

        private List<StarSystem> starSystems = new List<StarSystem>();
        private List<StarSystemConnection> starSystemConnections = new List<StarSystemConnection>();
        private List<Constellation> constellations = new List<Constellation>();

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

        public List<Constellation> Constellations
        {
            get
            {
                return constellations;
            }

            set
            {
                constellations = value;
            }
        }

        ///<summary>
        ///This is a description of my function.
        ///</summary>
        public void GenerateMap()
        {
            PlaceStarSystems();
            ConnectStarSystems();
            CreateConstellations();
        }

        void PlaceStarSystems()
        {
            List<Vector2> positions = new List<Vector2>();
            int starSystemCount = (int)UnityEngine.Random.Range(map.minSystems, map.maxSystems);

            for (int count = 0; count < starSystemCount;)
            {
                Vector2 position = new Vector2((int)UnityEngine.Random.Range((map.size.x / 2 - 2) * -1f, (map.size.x / 2 - 2)),
                                               (int)UnityEngine.Random.Range((map.size.y / 2 - 2) * -1f, (map.size.x / 2 - 2)));
                if (!isPositionSuitable(positions, position))
                    continue;

                if (UnityEngine.Random.Range(0f, 1f) > GetTemplateGreyscale(position.x, position.y))
                    continue;

                positions.Add(position);


                SystemType systemType = map.systemTypes.Find(GetSystemType).systemType;
                if (systemType == null)
                    systemType = map.systemTypes.Find((x) => x.systemType.useDefault).systemType;

                GameObject systemObject = systemType.gameObjects.Find((x) => UnityEngine.Random.Range(0f, 1f) <= x.probability).gameObject;
                if (systemObject == null)
                    systemObject = systemType.gameObjects[0].gameObject;
                systemObject = GameObject.Instantiate(systemObject);
                float size = UnityEngine.Random.Range(systemType.minSizeMultiplier, systemType.maxSizeMultiplier);
                systemObject.transform.GetChild(0).localScale = new Vector3(size, 1, size);
                
                string systemName = (starSystems.Count == 0 && systemType.names.Count > 0) ? systemType.names[0] : "System";
                foreach(StarSystem system in starSystems)
                {
                    systemName = systemType.names.Find((x) => (x != system.Name));
                    if (systemName == null)
                        systemName = "System"; //TODO: replace with default names
                }

                StarSystems.Add(new StarSystem(systemName, systemObject, position));
                count++;
            }
        }

        bool GetSystemType(SystemTypeAndProbability systemTypes)
        {
            if(UnityEngine.Random.Range(0f, 1f) <= systemTypes.probability)
            {
                if(!systemTypes.systemType.useDefault)
                    foreach(StarSystem system in starSystems)
                    {
                        if (systemTypes.systemType.names.Contains(system.Name))
                            return false;
                    }
                return true;
            }
            return false;
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

        void CreateConstellations()
        {
            List<StarSystem> systems = new List<StarSystem>(starSystems);
            for (int i = 0; i < systems.Count;)
            {
                List<StarSystem> constellation = GetConstellation(systems[i], new List<StarSystem>());
                foreach(StarSystem constellationSystem in constellation)
                {
                    if(systems.Contains(constellationSystem))
                        systems.Remove(constellationSystem);
                }
                constellations.Add(new Constellation("Constellation " + i, constellation));
            }
        }

        List<StarSystem> GetConstellation(StarSystem system, List<StarSystem> systems)
        {
            systems.Add(system);
            List<StarSystem> systemNeighbours = system.GetConnectedNeighbours();
            if (systemNeighbours?.Count != null )
            {
                foreach (StarSystem neighbour in systemNeighbours)
                {
                    if (systems.Contains(neighbour))
                        continue;
                    List<StarSystem> subresult = GetConstellation(neighbour, new List<StarSystem>(systems));
                    foreach(StarSystem subresultSystem in subresult)
                    {
                        if (systems.Contains(subresultSystem))
                            continue;
                        systems.Add(subresultSystem);
                    }
                }
            }
            return systems;
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