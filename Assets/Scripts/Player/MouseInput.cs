using Definitions;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class MouseInput
{
    /* fields */
    #region
    [SerializeField] private MouseInPut_Build _mouseInPut_Build = new();
    [SerializeField] private GatherItem gatherItem = new();
    [SerializeField] private Mining mining = new();
    public Mining Mining { get { return mining; } }
    public MouseInPut_Build MouseInPut_Build { get { return _mouseInPut_Build; } }
    public MouseInPut_Build MouseInPut_ { get { return _mouseInPut_Build; } }
    #endregion
    public void MouseInPutProcess()
    {
        //일반 상태일 때
        if (IsNomalState())
        {
            //좌클릭
            if (Input.GetKeyDown(PlayerScript.PlayerInstance.Key.LeftClick))
                LeftMouseInPut_NomalMethod();
            //우클릭
            if (Input.GetKeyDown(PlayerScript.PlayerInstance.Key.RightClick))
                RightMouseInPut_NomalMethod();
        }

        //건물 설치 상태일 때
        if (IsBuildState())
            MouseInPut_BuildingMethod();
    }
    /* codes */
    #region .
    void MouseInPut_BuildingMethod()
    {
        _mouseInPut_Build.BuildingProcess(RayfromTransform());
    }

    void LeftMouseInPut_NomalMethod()
    {
        Ray ray = RayfromTransform();
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, PlayerScript.instance.plMask.MaskMouseTrigger))
        {
            if (IsResource(hit))
                mining.MiningProcess(ray, hit);
            if (IsItem(hit))
                gatherItem.Gathering(hit);
        }
        //OnAttack();
    }
    /// <summary>
    /// 일반 상태일 때 실행되는 마우스 우클릭 이벤트 매소드
    /// </summary>
    void RightMouseInPut_NomalMethod()
    {
        /*
        Ray ray = RayfromTransform();
        if (myWeapon == WeaponType.Bow && Input.GetMouseButtonDown(1))
        {
            GameObject go = Instantiate(Resources.Load("Prefabs\\Arrow"), myAttackpos[(int)myWeapon].position,
                myAttackpos[(int)myWeapon].rotation, myAttackpos[(int)myWeapon]) as GameObject;

            _components.fpCamera.ScreenPointToRay(new Vector3(_components.fpCamera.pixelWidth / 2, _components.fpCamera.pixelHeight / 2));
            Debug.DrawRay(Com.rBody.position, myAttackpos[(int)myWeapon].rotation.eulerAngles, Color.red, 5f);

            myArrow = go?.GetComponent<Arrow>() ?? null;

            Com.anim.SetTrigger("BowShot");
        }
        */
    }
    /* codes */
    private bool IsBuildState()
    {
        return _mouseInPut_Build.IsBuildingStateActive;
    }
    private bool IsNomalState()
    {
        if (_mouseInPut_Build.IsBuildingStateActive) return false;
        return true;
    }
    /// <summary>캐릭터의 회전을 마우스 클릭 방향으로 정렬 후 크로스헤어 위치로 Ray를 발사</summary>///
    private Ray RayfromTransform()
    {
        if (!PlayerScript.PlayerInstance.State.isCurrentFp) PlayerScript.PlayerInstance.ApplyTpcamValueToFpcam();
        if (PlayerScript.PlayerInstance.State.isCurrentFp) PlayerScript.PlayerInstance.ApplyFpcamValueToTpcam();
        Ray ray = PlayerScript.PlayerInstance.Com.fpCamera.ScreenPointToRay(
            new Vector3(PlayerScript.PlayerInstance.Com.fpCamera.pixelWidth / 2, PlayerScript.PlayerInstance.Com.fpCamera.pixelHeight / 2));
        return ray;
    }

    private bool IsResource(RaycastHit hit)
    {
        return (1 << hit.transform.gameObject.layer == PlayerScript.instance.plMask.MaskResource);
    }
    private bool IsItem(RaycastHit hit)
    {
        return (1 << hit.transform.gameObject.layer == PlayerScript.instance.plMask.MaskItem);
    }
    #endregion
}