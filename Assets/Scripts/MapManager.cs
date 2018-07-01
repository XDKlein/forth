using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth {
    [Serializable]
    public class MapManager {
        public Vector2 mapSize = new Vector2(50f, 50f);
        public int minStarSystems = 20;
        public int maxStarSystems = 50;

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
            int starSystemCount = (int)UnityEngine.Random.Range(minStarSystems, maxStarSystems);
            for (int count = 0; count < starSystemCount;)
            {
                Vector2 position = new Vector2((int)UnityEngine.Random.Range((mapSize.x / 2 - 2) * -1f, (mapSize.x / 2 - 2)),
                                               (int)UnityEngine.Random.Range((mapSize.y / 2 - 2) * -1f, (mapSize.x / 2 - 2)));
                if (!isPositionSuitable(positions, position))
                    continue;

                positions.Add(position);
                StarSystems.Add(new StarSystem(count.ToString(), position));
                count++;
            }
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