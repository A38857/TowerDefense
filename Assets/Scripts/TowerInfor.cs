using System;
using UnityEngine;

[Serializable]
public class TowerInfor
{
    public string Name;
    public int Cost;
    public GameObject Prefab;

    // Method
    public TowerInfor(string TowerName, int TowerCost, GameObject TowerPrefab)
    {
        Name = TowerName;
        Cost = TowerCost;
        Prefab = TowerPrefab;
    }
}
