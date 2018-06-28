using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forth
{
    public class Planet : ScriptableObject
    {
        protected string planetName = "";
        protected int planetCapacity = 0;
        protected int planetBuildingSlots = 0;
        protected int planetPopulation = 0;
        protected List<Building> planetBUildings = new List<Building>();
    }
}