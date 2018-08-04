using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth {
    public class StarSystem : GameEntity
    {
        private string name = "System";
        private GameObject gameObject = null;
        //private Player systemOwner = null;
        private Vector2 position = new Vector2(0, 0);

        private Constellation constellation = null;
        private List<Planet> planets = new List<Planet>();

        private List<StarSystemConnection> connections = new List<StarSystemConnection>();
        private Dictionary<StarSystem, float> closestSystems = new Dictionary<StarSystem, float>();

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }

            set
            {
                gameObject = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Constellation Constellation
        {
            get
            {
                return constellation;
            }

            set
            {
                constellation = value;
            }
        }

        public List<Planet> Planets
        {
            get
            {
                return planets;
            }

            set
            {
                planets = value;
            }
        }

        public List<StarSystemConnection> Connections
        {
            get
            {
                return connections;
            }

            set
            {
                connections = value;
            }
        }

        public Dictionary<StarSystem, float> ClosestSystems
        {
            get
            {
                return closestSystems;
            }

            set
            {
                closestSystems = value;
            }
        }

        public StarSystem(string name, GameObject gameObject, List<Planet> planets, Vector2 position)
        {
            this.name = name;
            this.gameObject = gameObject;
            this.planets = planets;
            this.position = position;
        }

        public StarSystem(Vector2 position)
        {
            this.position = position;
        }

        public KeyValuePair<StarSystem, float> GetClosestSystem()
        {
            return ClosestSystems.First();
        }

        public bool IsConnectedTo(StarSystem system)
        {
            List<StarSystem> neighbours = this.GetConnectedNeighbours();
            return neighbours?.Contains(system) == true ? true : false;
        }

        public float DistanceTo(StarSystem system)
        {
            return Vector2.Distance(this.Position, system.Position);
        }

        public StarSystemConnection ConnectToClosest(List<StarSystem> starSystems)
        {
            float distance = int.MaxValue;
            StarSystem destination = null;
            
            foreach (StarSystem starSystem in starSystems)
            {
                if (this == starSystem)
                {
                    continue;
                }

                float calculatedDistance = this.DistanceTo(starSystem);
                ClosestSystems.Add(starSystem, calculatedDistance);

                if (this.IsIntersecting(starSystem, starSystems))
                {
                    continue;
                }

                if (calculatedDistance < distance)
                {
                    if (starSystem.IsConnectedTo(this))
                    {
                        continue;
                    }
                    else
                    {
                        distance = calculatedDistance;
                        destination = starSystem;
                    }
                }
            }
            ClosestSystems = ClosestSystems.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return destination != null ? new StarSystemConnection(this, destination) : null;
        }

        public bool IsIntersecting(StarSystem destination, List<StarSystem> systems)
        {
            foreach(StarSystem system in systems)
            {
                if (system != this && system != destination)
                {
                    if (Utility.PointToLineDistance(system.Position, this.Position, destination.Position) <= 1.5f)
                        return true;

                        foreach (StarSystemConnection connection in system.Connections)
                        {
                            if (connection.IsConnecting(this) || connection.IsConnecting(destination))
                                continue;
                            if (connection.IsIntersectableBy(this, destination))
                                return true;
                        }
                }
            }
            return false;
        }



        public List<StarSystem> GetConnectedNeighbours()
        {
            if (this.Connections.Count <= 0)
                return null;
            List<StarSystem> neighbours = new List<StarSystem>();
            foreach(StarSystemConnection connection in this.Connections)
            {
                neighbours.Add(connection.GetNeighbour(this));
            }
            return neighbours;
        }
    }
}