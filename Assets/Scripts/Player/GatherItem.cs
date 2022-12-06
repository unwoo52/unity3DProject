using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class GatherItem// : MonoBehaviour
{
    /* fields */
    #region .
    public static GroundItem handItem = null;
    #endregion
    public void Gathering(RaycastHit hit)
    {
        if (!TryGetScript(hit))         return;
        if (!IsReachedItem(hit))        return;

        if (PlayerScript.PlayerInstance.myInfo.curDelay >= PlayerScript.PlayerInstance.myInfo.MineDelay_Picking_Origin)
        {
            PlayerScript.PlayerInstance.myInfo.curDelay = 0.0f;
            PlayerScript.PlayerInstance.Com.anim.SetTrigger("TriggerGathering");
        }
    }
    /* codes */
    #region
    private bool IsReachedItem(RaycastHit hit)
    {
        return PlayerScript.PlayerInstance.myInfo.DistancePicking > Vector3.Distance(PlayerScript.PlayerInstance.transform.position, hit.point);
    }
    private bool TryGetScript(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out GroundItem grounsItem))
            handItem = grounsItem;
        else
        {
            Debug.LogError("아이템 오브젝트에 GroundItem가 존재하지 않습니다");
            return false;
        }
        return true;
    }
    public void TestSSScript()
    {

    }
    #endregion
}
