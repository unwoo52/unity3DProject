using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{

    public float atkBonus;      //장비 공격(데미지)설정
    public float defenceBonus;  //장비 방어(피해감소)설정
    private void Awake()
    {
        type = ItemType.Equipment;
    }

}
