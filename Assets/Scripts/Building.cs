using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Building")]
public class Building : ScriptableObject {
    //public buildingType = some default building;
    public string buildingName = "New Building";
    public int buildingExpenses = 100;

}
