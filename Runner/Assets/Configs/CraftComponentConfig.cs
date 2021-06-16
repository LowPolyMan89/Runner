using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Configs", menuName = "Configs/CraftComponentConfig", order = 1)]
public class CraftComponentConfig : ScriptableObject
{
    public string ComponentID;
    public List<CraftComponentData> needcraftComponents = new List<CraftComponentData>();
    public int Money;
    public float CraftTime;
    public int ComponentOutValue;
    public string BuildingId;
   
}


[System.Serializable]
public class CraftComponentData
{
    public string ComponentID;
    public int Value;
}
