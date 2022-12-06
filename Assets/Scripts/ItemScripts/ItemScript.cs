using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [Space, Header(" # 장비 정보")]
    public ItemStat stat;
    //타입 코드 ex)도끼,양손검,곡갱이
    [SerializeField] private int itemTypeCode;
    public int ItemTypeCode { get { return itemTypeCode; }}
    //장착 위치 코드 ex)0:오른손, 1:헬멧
    [SerializeField] private int itemSlotCode;
    public int ItemSlotCode { get { return itemSlotCode; } }


    //장비 버프 값
    [SerializeField] private List<string> gadgetBuffTypeName;
    public List<string> GadgetBuffTypeName { get { return gadgetBuffTypeName; }}
    [SerializeField] private List<float> gadgetBuffvalue;
    public List<float> GadgetBuffvalue { get { return gadgetBuffvalue; }}
    private void Start()
    {
        stat = new ItemStat(
            itemType : itemTypeCode);
    }
}
