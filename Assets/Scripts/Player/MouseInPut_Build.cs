using Definitions;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MouseInPut_Build// : MonoBehaviour
{
    /* fields */
    #region .
    private bool isBuildingStateActive = false;
    public bool IsBuildingStateActive { get { return isBuildingStateActive; } }
    private float distanceBuilding = 1.0f;
    private GameObject HandBuilding; //건축 상태일 때 건물 오브젝트를 보관하는 공간
    private BuildingObjectScript HandBuildingScript;
    private Renderer HandBuildingRenderer;
    #endregion
    public void BuildingProcess(Ray ray)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelBuilding();
            return;
        }

        //레이가 땅에 닿는다면
        if (Physics.Raycast(ray, out RaycastHit hit, distanceBuilding, PlayerScript.instance.plMask.MaskTerrain))
        {
            RotateBuilding_to_HitPosition(hit.point);

            if (IsCollOverlap())    return;
            if (!IsCanBuild())      return;

            if (Input.GetMouseButton(0))
                BuildProcess(hit);
        }
        else
        {
            RotateBuilding_to_RayEnd(ray);
        }
    }

    /* codes */
    #region .
    private void RenderRed()
    {
        return;
    }
    private void RenderGreen()
    {
        return;
    }
    private void RenderCyan()
    {
        return;
    }
    private void RotateBuilding_to_RayEnd(Ray ray)
    {
        HandBuilding.transform.position = ray.GetPoint(distanceBuilding);
        RenderCyan();
    }
    public void BuildProcess(RaycastHit hit)
    {
        isBuildingStateActive = false;
        HandBuilding.GetComponent<Collider>().isTrigger = false;
        HandBuilding.transform.position = hit.point;
        HandBuilding.transform.parent = null;
        HandBuilding.GetComponentInChildren<Renderer>().sharedMaterial.color = Color.white;
        HandBuildingScript.ChangeState(BuildingObjectScript.BuildingObjectState.Runing);
        HandBuilding = null;
    }
    private bool IsCollOverlap()
    {
        if(HandBuildingScript.isBoxOverlap)
        {
            RenderRed();
            return true;
        }
        return false;
    }
    private bool IsCanBuild()
    {
        if (HandBuildingScript.isBoxOverlap)    return false;
        RenderGreen();
        return true;
    }
    private void RotateBuilding_to_HitPosition(Vector3 vector3)
    {
        HandBuilding.transform.position = vector3;
    }
    private void CancelBuilding()
    {
        HandBuildingRenderer.sharedMaterial.color = Color.white;
        HandBuildingScript.ChangeState(BuildingObjectScript.BuildingObjectState.Destroy);
        HandBuilding = null;
        isBuildingStateActive = false;
    }
    public void StartBuilding(GameObject gameObject)
    {
        if (gameObject is null)
        {
            throw new ArgumentNullException(nameof(gameObject));
        }
        PlayerScript.PlayerInstance.ActiveOnCursor();
        isBuildingStateActive = true;

        HandBuilding.transform.SetParent(PlayerScript.PlayerInstance.transform);
        gameObject.transform.GetChild(0).TryGetComponent(out HandBuildingScript);
    }
    #endregion
}
