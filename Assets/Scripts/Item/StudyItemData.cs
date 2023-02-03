using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class StudyItemData : ScriptableObject
{
    public string Name;
    public int Price;
    public int Value;
}
