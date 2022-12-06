using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStat
{
    private int itemType;
    public int ItemType { get { return itemType; } }
    public ItemStat(int itemType)
    {
        this.itemType = itemType;
    }
}
