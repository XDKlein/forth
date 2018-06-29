using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth {
    [Serializable]
    public class StarSystem
    {
        private string systemName = "System";
        private GameObject systemGameObject = null;
        //private Player systemOwner = null;
        private Vector2 systemPosition = new Vector2(0, 0);

        private StarSector systemSector = null;
        private List<Planet> systemPlanets = new List<Planet>();

        private List<StarSystemConnection> systemConnections = new List<StarSystemConnection>();

        public StarSystem(Vector2 systemPosition)
        {
            this.systemPosition = systemPosition;
        }

        public string GetSystemName()
        {
            return this.systemName;
        }

        public void SetSystemName(string systemName)
        {
            this.systemName = systemName;
        }

        public GameObject GetSystemGameObject()
        {
            return systemGameObject;
        }
        public void SetSystemGameObject(GameObject systemGameObject)
        {
            this.systemGameObject = systemGameObject;
        }

        public Vector2 GetSystemPosition()
        {
            return systemPosition;
        }

        public void SetSystemConnections(List<StarSystemConnection> systemConnections)
        {
            this.systemConnections = systemConnections;
        }

        public void AddSystemConnection(StarSystemConnection systemConnection)
        {
            systemConnections.Add(systemConnection);
        }

        public List<StarSystemConnection> GetSystemConnections()
        {
            return systemConnections;
        }

        public bool IsConnectedTo(StarSystem system)
        {
            List<StarSystem> neighbours = this.GetConnectedNeighbours();
            return neighbours?.Contains(system) == true ? true : false;
        }

        public float DistanceTo(StarSystem system)
        {
            return Vector2.Distance(this.GetSystemPosition(), system.GetSystemPosition());
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
                foreach(StarSystemConnection connection in system.systemConnections)
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
            if (systemConnections.Count <= 0)
                return null;
            List<StarSystem> neighbours = new List<StarSystem>();
            foreach(StarSystemConnection connection in systemConnections)
            {
                neighbours.Add(connection.GetNeighbour(this));
            }
            return neighbours;
        }
    }
}