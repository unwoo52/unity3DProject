using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Mining
{
    /* fields */
    #region .
    private RockScript _rockScript;
    private TreeScript _treeScript;
    public RockScript RockScript => _rockScript;
    public TreeScript TreeScript => _treeScript;
    private Ray rockHitRay;
    private Vector3 rockHitVector;
    public Ray RockHitRay => rockHitRay;
    public Vector3 RockHitVector => rockHitVector;
    private float RockDmg;
    #endregion

    public void MiningProcess(Ray ray, RaycastHit hit)
    {
        if (!IsDelayReady())            return;
        if (!IsReachedDistance(hit))    return;

        if (hit.transform.TryGetComponent<TreeScript>(out var trees))
            DoMine(trees);

        if (hit.transform.TryGetComponent<RockScript>(out var rocs))
            DoLogging(rocs, ray, hit);
    }

    /* callback Method */
    public void DoMine(TreeScript trees)
    {
        PlayerScript.PlayerInstance.Com.anim.SetTrigger("TriggerHaning");
        PlayerScript.PlayerInstance.myInfo.curDelay = 0.0f;

        _treeScript = trees;
    }
    public void DoLogging(RockScript rocs, Ray ray, RaycastHit hit)
    {
        PlayerScript.PlayerInstance.Com.anim.SetTrigger("TriggerMine");
        PlayerScript.PlayerInstance.myInfo.curDelay = 0.0f;

        _rockScript = rocs;
        rockHitRay = ray;
        rockHitVector = hit.point;
    }
    /* codes */
    #region .
    private Vector3 GetPlayerPosition()
    {
        return PlayerScript.PlayerInstance.GetPlayerPosition();
    }
    private bool IsDelayReady()
    {
        return PlayerScript.PlayerInstance.myInfo.curDelay >= PlayerScript.PlayerInstance.myInfo.MineDelay_Logging_Origin;
    }
    private bool IsReachedDistance(RaycastHit hit)
    {
        return (PlayerScript.PlayerInstance.myInfo.DistanceMining > Vector3.Distance(GetPlayerPosition(), hit.point));
    }
    /* anim event */
    #endregion
}