using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Definitions;

public class TerrainSystem : MonoBehaviour
{
    [SerializeField] LayerMask MaskPlayer;
    [SerializeField] int terrainLayerMaskInt;
    int TerrainLayerRange = 29;//terrain layer min value

    private void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & MaskPlayer) != 0)
        {
            PlayerScript playerinstance = PlayerScript.instance;
            //플레이어의 curTerrainLayer가 현재 terrain의 layer와 일치하는가
            if ((playerinstance.plMask.currentTerrainLayer ^ terrainLayerMaskInt) != 0)
            {
                //플레이어 curLayer에 terrain의 Layer 대입
                playerinstance.plMask.currentTerrainLayer.value = terrainLayerMaskInt;
                //기존 지형 버프 파괴
                foreach (BaseBuff baseBuff in FindCurNoneAxisTerrainBuff(playerinstance.BuffList, playerinstance.plMask.currentTerrainLayer))
                {
                    baseBuff.BuffDeActivation();
                }
                //새 지형 버프 적용
                foreach (int i in FindCurAxisTerrainBuff(playerinstance.BuffList, playerinstance.plMask.currentTerrainLayer))
                {
                    AddTerrainBuffActive(i);
                }
            }
        }
    }

    /// <summary>
    /// 플레이어의 curTerrainLayer에서는 활성화인데 버프가 없는 Terrain레이어의 번호를 반환.
    /// 플레이어가 31 30 지형에 있는데 31 버프만 있다면 30(int)를 리턴
    /// </summary>
    /// <param name="buffList">Player BuffList</param>
    /// <param name="layer">Player Current Terrain Layer</param>
    /// <returns>추가해야 할 버프가 있는 Terrain Layer Value를 리턴</returns>
    List<int> FindCurAxisTerrainBuff(List<BaseBuff> buffList , LayerMask layer) 
    {
        List<int> llist = new();
        for (int i = TerrainLayerRange; i < 32; i++)//TerrainLayer 범위
        {
            if((layer & 1 << i) != 0) //플레이어가 0011 이라면 1과 2번 레이어에 대해서 함수 실행
            {
                if(FindTerrainPassive(buffList, i) == null) llist.Add(i);
            }
        }
        return llist;
    }

    /// <summary>
    /// 플레이어의 curTerrainLayer에 존재하지 않는 레이어 번호임에도 버프가 있다면 해당 버프들을 모두 리스트에 저장해 리턴.
    /// 플레이어가 31 30 지형에 있는데 31 30 29 버프가 있다면 29만 리턴
    /// </summary>
    /// <param name="buffList">Player BuffList</param>
    /// <param name="layer">Player Current Terrain Layer</param>
    /// <returns>삭제해야 할 버프 인스턴스가 담긴 리스트</returns>
    List<BaseBuff> FindCurNoneAxisTerrainBuff(List<BaseBuff> buffList, LayerMask layer)
    {
        List<BaseBuff> list = new();
        for (int i = TerrainLayerRange; i < 32; i++)//TerrainLayer 범위
        {
            if ((~layer ^ 1 << i) != 0) //cur레이어의 i번째 비트가 0일 때 버프 탐색 실행
            {
                BaseBuff findedBuff = FindTerrainPassive(buffList, i);
                if (findedBuff != null) list.Add(findedBuff);
            }
        }
        return list;
    }

    /// <summary>
    /// 플레이어의 버프 리스트와 찾아야 할 레이어 번호를 받아 해당 번호의 버프를 탐색해서 리턴
    /// </summary>
    /// <param name="BuffList">버프 리스트</param>
    /// <param name="buffcode">탐색할 버프 레이어 번호</param>
    /// <returns></returns>
    BaseBuff FindTerrainPassive(List<BaseBuff> BuffList, int buffcode)// 패시브 버프를 탐색해 반환
    {
        if (BuffList.Count > 0)
        {
            for (int i = 0; i < BuffList.Count; i++)//버프 갯수만큼 반복, 버프 리스트를 훑기
            {
                if (BuffList[i].BuffCode == buffcode) return BuffList[i];
            }
        }return null;
    }


    void AddTerrainBuffActive(int terrainLayerNumber)
    {
        List<string> buffTypenameList = new();
        List<float> buffValueList = new();
        int buffCode = terrainLayerNumber;
        Sprite icon;

        switch (terrainLayerNumber)
        {
            case 29:
                buffTypenameList.Add("MoveSpeed");
                buffValueList.Add(0.3f);
                icon = Resources.Load("BuffImage/11_Melee_Cone", typeof(Sprite)) as Sprite;
                BuffManagerScript.instance.CreateBuff(buffTypenameList, buffValueList, icon, buffCode);
                break;
            case 30:
                buffTypenameList.Add("MoveSpeed");
                buffValueList.Add(0.3f);
                icon = Resources.Load("BuffImage/02_Fire", typeof(Sprite)) as Sprite;
                BuffManagerScript.instance.CreateBuff(buffTypenameList, buffValueList, icon, buffCode);
                break;
            case 31:
                buffTypenameList.Add("MoveSpeed");
                buffTypenameList.Add("MineDelay_Mining");
                buffValueList.Add(-0.3f);
                buffValueList.Add(0.3f);
                icon = Resources.Load("BuffImage/04_Ice_Nova", typeof(Sprite)) as Sprite;
                BuffManagerScript.instance.CreateBuff(buffTypenameList, buffValueList, icon, buffCode);
                break;
        }
    }
}
