using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class Planet : ScriptableObject
    {
        protected string name = "";
        protected int capacity = 0;
        protected int buildingSlots = 0;
        protected int population = 0;
        protected List<Building> buildings = new List<Building>();

        public Planet(string name)
        {
            this.name = name;
        }
    }
}