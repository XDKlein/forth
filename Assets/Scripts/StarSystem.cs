using System.Collections;
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

        private StarSector sector = null;
        private List<Planet> planets = new List<Planet>();

        private List<StarSystemConnection> connections = new List<StarSystemConnection>();

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

        public StarSector Sector
        {
            get
            {
                return sector;
            }

            set
            {
                sector = value;
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

        public StarSystem(Vector2 position)
        {
            this.position = position;
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
                if (this == starSystem || this.IsIntersecting(starSystem, starSystems))
                    continue;

                float calculatedDistance = this.DistanceTo(starSystem);
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
            return destination != null ? new StarSystemConnection(this, destination) : null;
        }

        public bool IsIntersecting(StarSystem destination, List<StarSystem> systems)
        {
            foreach(StarSystem system in systems)
            {
                foreach(StarSystemConnection connection in system.Connections)
                {
                    if (connection.IsConnecting(this) || connection.IsConnecting(destination))
                        continue;
                    if (connection.IsIntersectableBy(this, destination))
                        return true;
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