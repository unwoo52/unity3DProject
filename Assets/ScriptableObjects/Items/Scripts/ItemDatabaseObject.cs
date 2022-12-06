using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New ItemDatabase", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver // ISerializationCallbackReceiver - 직렬화 역직렬화 수동처리 콜백용
{
    
    public ItemObject[] Items;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < Items.Length; i++)
        {
            Items[i].Id = i;
            GetItem.Add(i, Items[i]);
        }
    }
    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
    
    
}
