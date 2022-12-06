using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Object", menuName = "Inventory System/Items/Building")]
public class BuildingObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Building;
    }
}
