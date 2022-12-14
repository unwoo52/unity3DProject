using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using static UnityEditor.Progress;
using System.Security.Cryptography;
using static UnityEditorInternal.ReorderableList;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;
    

    public void AddItem(Item _item, int _amount)
    {
        if(_item.buffs.Length > 0)
        {
            SetEmptySlot(_item,_amount);
            return;
        }
        for(int i = 0; i < Container.Items.Length; i++) // 인벤토리안에 아이템이있는지 반복검사
        {
            if (Container.Items[i].ID == _item.Id)      // i번째 슬롯의 아이템이 현재 확인할 아이템과 동일할경우
            {
                Container.Items[i].AddAmount(_amount); // 해당슬롯의 아이템에 수량추가
                return;                          
            }
        }
        SetEmptySlot(_item, _amount);
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id,_item.ItemType, _item, _amount);
                return Container.Items[i];
            }
        }
        return null;
    }
    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID,item2.type, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID,item1.type, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID,temp.type, temp.item, temp.amount);
    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1,ItemType.None, null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save() //아이템 저장 저장경로 - 저장 경로 : C:\Users\[user name]\AppData\DefaultCompany\AtentsGclassAteam
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(String.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load() //아이템불러오기
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for(int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].type, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    { 
        Container = new Inventory();
    }

}

[Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[9]; //아이템 오브젝트 리스트 생성
}
[Serializable]
public class InventorySlot
{
    public int ID = -1;
    public ItemType type;
    public Item item; //슬롯에 넣을 아이템오브젝트
    public int amount;      //수량
    public InventorySlot() // 슬롯 기본 초기화설정
    {
        ID = -1;
        type = ItemType.None;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id,ItemType _type, Item _item, int _amount)
    {
        ID = _id;
        type = _type;
        item = _item;
        amount = _amount;
    }
    public void UpdateSlot(int _id, ItemType _type, Item _item, int _amount) 
    {
        ID = _id;
        type = _type;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
