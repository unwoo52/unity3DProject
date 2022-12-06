using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{   //  장비 / 음식 / 기본 / 건축물
    None,
    Default,
    Equipment,
    Food,
    Building
}
public enum Attributes
{
    Agility, // 민첩 DEX
    Intellect, // 지력 INT
    Stamina, // 지구력 Hp
    strngth //힘 STR
}
public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;     // 게임오브젝트이미지
    public GameObject prefab;    // 게임오브젝트
    public ItemType type;         // 아이템 타입
    [TextArea(15, 20)]
    public string description;    // 아이템 설명
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}
[System.Serializable]
public class Item
{
    public string Name;
    public ItemType ItemType;
    public int Id;
    public ItemBuff[] buffs;
    public Item(ItemObject item)
    {
        Name = item.name;
        ItemType = item.type;
        Id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max);
            buffs[i].attributes = item.buffs[i].attributes;
        }
    }
}
[System.Serializable]
public class ItemBuff
{
    public Attributes attributes;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max); // 시스템밖에서 랜덤을 선언할경우 유니티엔진을 적지않으면 모호성때문에 에러가 난다
    }
}
