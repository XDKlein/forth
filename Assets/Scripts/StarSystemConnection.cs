﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth
{
    [Serializable]
    public class StarSystemConnection
    {
        private StarSystem[] starSystems = new StarSystem[2];
        private GameObject connectionObject;
        private float connectionLength;

        public StarSystemConnection(StarSystem aStarSystem, StarSystem bStarSystem)
        {
            starSystems[0] = aStarSystem;
            starSystems[1] = bStarSystem;
            aStarSystem.AddSystemConnection(this);
            bStarSystem.AddSystemConnection(this);
        }

        public GameObject GetConnectionGameObject()
        {
            return connectionObject;
        }

        public void SetConnectionGameObject(GameObject connectionObject)
        {
            this.connectionObject = connectionObject;
        }

        public StarSystem GetNeighbour(StarSystem origin)
        {
            StarSystem destination = null;
            if (starSystems.Length > 0)
            {
                foreach (StarSystem system in starSystems)
                {
                    if (system == origin)
                        continue;
                    else
                    {
                        destination = system;
                        break;
                    }
                }
            }
            return destination;
        }
        public bool IsConnecting(StarSystem system)
        {
            return Array.IndexOf(starSystems, system) >= 0;
        }

        public StarSystem[] GetStarSystems()
        {
            return starSystems;
        } 

        public bool IsIntersectableBy(StarSystem origin, StarSystem destination)
        {
            return Utility.LineIntesection(this.starSystems[0].GetSystemPosition(), this.starSystems[1].GetSystemPosition(), origin.GetSystemPosition(), destination.GetSystemPosition());
        }
    }
}
