using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBuffPenal : MonoBehaviour
{
    [SerializeField] LayerMask MaskPlayer;
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & MaskPlayer) != 0)
        {
            OnBuff();
        }
    }

    void OnBuff()
    {
        List<string> buffTypenameList = new() { "MoveSpeed", "MineDelay_Mining" };
        List<float> buffValueList = new() { 0.5f, -0.7f };
        BuffManagerScript.instance.CreateBuff(buffTypenameList, buffValueList, 5.0f, Resources.Load("BuffImage/14_Summon", typeof(Sprite)) as Sprite);
    }
}
