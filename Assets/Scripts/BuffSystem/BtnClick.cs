using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClick : MonoBehaviour
{
    //������ ȿ�� ���� �̸�(���ݷ� �����, ���� �����)�� �޾ƿ� ���� ex)��������� "����"
    public string buffTypename;
    public List<string> buffTypenameList = new();

    //��ŭ�� ���� ��ȭ�������� ex)��������� ���� "-10%"
    public float buffValue;
    public List<float> buffValueList = new();

    //���� �ð�
    public float buffOriginTime;

    public Sprite icon; //Ÿ�̸�. ���⿡ ���� �̹��� ���� ex)"��������� ������"
    public void Click()
    {
        //terrain�� terrainScirpt�� oncollisionenter�� �Ű� ���. CreateBuff(()=>, iceValue, sprite(ResourceLoadAll))
        //
        //BuffManagerScript.instance.CreateBuff(buffTypename, buffValue, buffOriginTime, GetComponent<UnityEngine.UI.Image>().sprite);

        //or
        

    }

    public void ClicktoDoubleBuff()
    {
        BuffManagerScript.instance.CreateBuff(buffTypenameList, buffValueList, buffOriginTime, GetComponent<UnityEngine.UI.Image>().sprite);
    }



    //BuffManagerScript.instance.CreateBuff(ICE, 0.5, ()=>exit Collider , pubilc Sprite IceFieldIcon = null;//�̹��� �־�α�)
}
