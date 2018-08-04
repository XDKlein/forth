using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth
{
    public class Constellation
    {
        private string name = "Constellation";
        private List<StarSystem> systems = new List<StarSystem>();
        private Vector2 centroid = new Vector2(0, 0);
        private List<Constellation> connectedConstellation = new List<Constellation>();

        private Border border;
        public List<Vector2> oldBorder;

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

        public List<StarSystem> Systems
        {
            get
            {
                return systems;
            }

            set
            {
                systems = value;
            }
        }

        public Vector2 Centroid
        {
            get
            {
                return centroid;
            }

            set
            {
                centroid = value;
            }
        }

        public List<Constellation> ConnectedConstellation
        {
            get
            {
                return connectedConstellation;
            }

            set
            {
                connectedConstellation = value;
            }
        }

        public Border Border
        {
            get
            {
                return border;
            }

            set
            {
                border = value;
            }
        }

        public Constellation(string name, List<StarSystem> systems)
        {
            this.name = name;
            this.systems = systems;
            foreach (StarSystem system in systems)
            {
                system.Constellation = this;
            }
            this.centroid = CalculateGeometricCenter();
            Border = new Border(this);  
        }

        public Vector2 CalculateGeometricCenter()
        {
            //Vector2 summary = new Vector2();
            Vector2 farLeft = systems[0].Position;
            Vector2 farRight = systems[0].Position;
            Vector2 farUp = systems[0].Position;
            Vector2 farDown = systems[0].Position;
            foreach (StarSystem system in systems)
            {
                if (farLeft.x > system.Position.x)
                    farLeft = system.Position;
                if (farRight.x < system.Position.x)
                    farRight = system.Position;
                if (farDown.y > system.Position.y)
                    farDown = system.Position;
                if (farUp.y < system.Position.y)
                    farUp = system.Position;
                //summary += system.Position;
            }
            return (farLeft + farRight + farDown + farUp) / 4; // summary / systems.Count;
        }

        public float GetHeight()
        {
            Vector2 farUp = systems[0].Position;
            Vector2 farDown = systems[0].Position;
            foreach (StarSystem system in systems)
            {
                if (farDown.y > system.Position.y)
                    farDown = system.Position;
                if (farUp.y < system.Position.y)
                    farUp = system.Position;
            }
            return Vector2.Distance(farDown, farUp);
        }

        public float GetWidth()
        {
            Vector2 farLeft = systems[0].Position;
            Vector2 farRight = systems[0].Position;
            foreach (StarSystem system in systems)
            {
                if (farLeft.x > system.Position.x)
                    farLeft = system.Position;
                if (farRight.x < system.Position.x)
                    farRight = system.Position;
            }
            return Vector2.Distance(farLeft, farRight);
        }

        public float GetAngle()
        {
            Vector2 farUp = systems[0].Position;
            Vector2 farDown = systems[0].Position;
            foreach (StarSystem system in systems)
            {
                if (farDown.y > system.Position.y)
                    farDown = system.Position;
                if (farUp.y < system.Position.y)
                    farUp = system.Position;
            }
            return Vector2.Angle(farUp, farDown);
        }
        

    }
}