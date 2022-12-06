using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClick : MonoBehaviour
{
    //버프의 효과 종류 이름(공격력 디버프, 방어력 디버프)을 받아올 변수 ex)디버프오라 "방어력"
    public string buffTypename;
    public List<string> buffTypenameList = new();

    //얼마큼의 값을 변화시켜줄지 ex)디버프오라 방어력 "-10%"
    public float buffValue;
    public List<float> buffValueList = new();

    //버프 시간
    public float buffOriginTime;

    public Sprite icon; //타이머. 여기에 버프 이미지 보관 ex)"디버프오라 아이콘"
    public void Click()
    {
        //terrain은 terrainScirpt의 oncollisionenter로 옮겨 사용. CreateBuff(()=>, iceValue, sprite(ResourceLoadAll))
        //
        //BuffManagerScript.instance.CreateBuff(buffTypename, buffValue, buffOriginTime, GetComponent<UnityEngine.UI.Image>().sprite);

        //or
        

    }

    public void ClicktoDoubleBuff()
    {
        BuffManagerScript.instance.CreateBuff(buffTypenameList, buffValueList, buffOriginTime, GetComponent<UnityEngine.UI.Image>().sprite);
    }



    //BuffManagerScript.instance.CreateBuff(ICE, 0.5, ()=>exit Collider , pubilc Sprite IceFieldIcon = null;//이미지 넣어두기)
}
