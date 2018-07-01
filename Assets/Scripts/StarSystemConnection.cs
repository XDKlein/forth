using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth
{
    public class StarSystemConnection
    {
        private StarSystem[] starSystems = new StarSystem[2];
        private GameObject gameObject;
        private float length;

        public StarSystem[] StarSystems
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

        public float ConnectionLength
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
            }
        }

        public StarSystemConnection(StarSystem aStarSystem, StarSystem bStarSystem)
        {
            StarSystems[0] = aStarSystem;
            StarSystems[1] = bStarSystem;
            aStarSystem.Connections.Add(this);
            bStarSystem.Connections.Add(this);
        }

        public StarSystem GetNeighbour(StarSystem origin)
        {
            StarSystem destination = null;
            if (StarSystems.Length > 0)
            {
                foreach (StarSystem system in StarSystems)
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
            return Array.IndexOf(StarSystems, system) >= 0;
        }

        public bool IsIntersectableBy(StarSystem origin, StarSystem destination)
        {
            return Utility.LineIntesection(this.StarSystems[0].Position, this.StarSystems[1].Position, origin.Position, destination.Position);
        }
    }
}
