using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth
{
    [Serializable]
    public class StarSector
    {
        private string starSectorName = "Sector";
        private List<StarSystem> starSectorSystems = new List<StarSystem>();
        private Vector2 starSectorGeometricCenter = new Vector2(0, 0);
        private List<StarSector> connectedStarSectors = new List<StarSector>();

        public string StarSectorName
        {
            get
            {
                return starSectorName;
            }

            set
            {
                starSectorName = value;
            }
        }

        public List<StarSystem> StarSectorSystems
        {
            get
            {
                return starSectorSystems;
            }

            set
            {
                starSectorSystems = value;
            }
        }

        public Vector2 StarSectorGeometricCenter
        {
            get
            {
                return starSectorGeometricCenter;
            }

            set
            {
                starSectorGeometricCenter = value;
            }
        }

        public List<StarSector> ConnectedStarSectors
        {
            get
            {
                return connectedStarSectors;
            }

            set
            {
                connectedStarSectors = value;
            }
        }


    }
}