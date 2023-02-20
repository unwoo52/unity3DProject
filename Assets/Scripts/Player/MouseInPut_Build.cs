using Definitions;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
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
    private float distanceBuilding = 10.0f;
    private GameObject HandBuilding; //건축 상태일 때 건물 오브젝트를 보관하는 공간
    private BuildingObjectScript HandBuildingScript;


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
            RenderGreen();

            if (Input.GetMouseButton(0))
                BuildProcess(hit);
        }
        else
        {
            RotateBuilding_to_RayEnd(ray);
            RenderCyan();
        }
    }

    /* codes */
    #region .
    private void SwapRenderer()
    {
        Material material = Resources.Load("Prefabs/imsi") as Material;
        HandBuildingScript.ChangeMaterials(material);
        if (HandBuildingScript.GetMaterial() == null) 
        { 
            Debug.LogError("임시 Metarial로 바꾸는 데에 실패했습니다.");
            HandBuildingScript.SetOrigibMaterail();
        }
    }
    private void RenderRed()
    {
        HandBuildingScript.GetMaterial().color = new Color(207 / 255f,0,0, 61 / 255f);
        return;
    }
    private void RenderGreen()
    {
        HandBuildingScript.GetMaterial().color = new Color(102 / 255f, 233 / 255f, 52 / 255f, 75 / 255f);
        return;
    }
    private void RenderCyan()
    {
        HandBuildingScript.GetMaterial().color = new Color(99/255f, 99 / 255f, 69 / 255f, 71 / 255f);
        return;
    }
    private void RotateBuilding_to_RayEnd(Ray ray)
    {
        HandBuilding.transform.position = ray.GetPoint(distanceBuilding);
    }
    public void BuildProcess(RaycastHit hit)
    {
        RollbackMaterial();
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
    private void RotateBuilding_to_HitPosition(Vector3 vector3)
    {
        HandBuilding.transform.position = vector3;
    }
    private void CancelBuilding()
    {
        RollbackMaterial();
        HandBuildingScript.ChangeState(BuildingObjectScript.BuildingObjectState.Destroy);
        HandBuilding = null;
        isBuildingStateActive = false;
    }
    public void StartBuilding(GameObject gameObject)
    {
        HandBuilding = gameObject;
        HandBuilding.transform.GetChild(0).TryGetComponent(out HandBuildingScript);
        if (HandBuilding is null)
        {
            throw new ArgumentNullException(nameof(gameObject));
        }
        if (HandBuildingScript is null)
        {
            throw new ArgumentNullException(nameof(gameObject));
        }

        PlayerScript.PlayerInstance.ActiveOnCursor();
        isBuildingStateActive = true;

        HandBuildingScript.ChangeState(BuildingObjectScript.BuildingObjectState.Making);
        HandBuilding.transform.SetParent(PlayerScript.PlayerInstance.transform);
        gameObject.transform.GetChild(0).TryGetComponent(out HandBuildingScript);
        SwapRenderer();
    }

    private void RollbackMaterial()
    {
        
        if(!(HandBuildingScript.ChangeMaterials(HandBuildingScript._OriginMaterial)))
        {
            Debug.LogError("Material을 Origin Material로 돌려놓는데에 실패했습니다.");
        }
    }
    #endregion
}
