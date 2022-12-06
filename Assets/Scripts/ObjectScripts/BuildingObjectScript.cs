using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObjectScript : MonoBehaviour
{
    #region fields, methods
    public bool isDistanceOver = false; //레이 끝까지 가도 건설 가능한 바닥이 없어서 건설할 수 없는 상태인가?
    public bool isBoxOverlap = false; //checkBox가 벽에 닿아서 건설할 수 없는 상태인가?

    private Transform buildingObject; //자기 자신 스크립트 저장
    private MeshCollider RootObjectMeshcollider;
    private GameObject Root; //이 오브젝트의 root 오브젝트 저장
    private GameObject OverlapObject;
    private GameObject EffectObject; //campfire의 경우엔 fire 오브젝트, 버프나 효과 등을 가지는 형제 오브젝트
    #endregion
    #region Initialize
    private void Awake()
    {
        HierarchySetting();
    }
    /// <summary>
    /// buildingObject, Root를 선언하고 effectObject를 비활성화
    /// </summary>
    private void HierarchySetting()
    {
        TryGetComponent(out buildingObject);
        Root = buildingObject.parent.gameObject;
        if (Root.transform.GetChild(1).gameObject)
        {
            EffectObject = Root.transform.GetChild(1).gameObject;
            EffectObject.SetActive(false);
        }
        else Debug.LogError("건물에서 effectObject가 없습니다");

        if(this.transform.GetChild(0).TryGetComponent(out CheckBoxScript _))
        {
            OverlapObject = this.transform.GetChild(0).gameObject;
            OverlapObject.SetActive(false);
        }
        else Debug.LogError("건물에 OverlapObject가 없습니다");

        if (Root.TryGetComponent(out MeshCollider RootObjectMeshcollider)) RootObjectMeshcollider.enabled = false;
        else Debug.LogError("건물에 MeshCollider가 없습니다");
    }
    #endregion
    #region State Machine
    public enum BuildingObjectState { Create, Item, Making, Runing, Destroy }
    public BuildingObjectState campFireState = BuildingObjectState.Create;
    public void ChangeState(BuildingObjectState state)
    {
        if (campFireState == state) return;
        switch (state)
        {
            case BuildingObjectState.Create:
                break;
            case BuildingObjectState.Item:
                break;
            case BuildingObjectState.Making:
                break;
            case BuildingObjectState.Runing:
                OverlapObject.SetActive(true);
                EffectObject.SetActive(true);
                break;
            case BuildingObjectState.Destroy:
                Destroy(transform.parent.gameObject);
                Destroy(this);
                break;
        }
    }
    #endregion
    /* codes */
}