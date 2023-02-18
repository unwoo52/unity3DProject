using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    [SerializeField] LayerMask TerrainMasks;
    GameObject DropItemGraphic;
    float curPos;
    float downMax = 1f;
    float upMax = 1f;
    float Movespeed = 0.1f;

    void Start()
    {
        curPos = transform.position.y;
        if(!CrateDropItem()) Debug.LogError("드롭 아이템 생성 실패");
    }
    void Update()
    {
        ItemHovering();
    }

    /* codes */

    private void ItemHovering()
    {
        float delta = Time.deltaTime * Movespeed;
        curPos += delta;
        if (curPos <= downMax)
        {
            Movespeed *= -1f;
            curPos = downMax;
        }
        if (curPos >= upMax)
        {
            Movespeed *= -1f;
            curPos = upMax;
        }
        transform.Translate(0, curPos * delta, 0);
    }
    private bool CrateDropItem()
    {
        DropItemGraphic = new GameObject(item.name);

        if (!AddMeshFilter(DropItemGraphic)) return false;
        if (!AddMeshRenderer(DropItemGraphic)) return false;

        DropItemGraphic.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        DropItemGraphic.transform.position = this.transform.position;
        DropItemGraphic.transform.SetParent(transform);

        return true;
    }

    

    private bool AddMeshFilter(GameObject obj)
    {
        if (item.prefab.gameObject.TryGetComponent(out MeshFilter meshfilter))
        {
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            mf.mesh = meshfilter.sharedMesh;
            obj.AddComponent<MeshRenderer>();
            return true;
        }
        else Debug.LogError("아이템 프레펩에 MeshFilter가 존재하지 않습니다.");     return false;
    }
    private bool AddMeshRenderer(GameObject obj)
    {
        MeshRenderer prefabMesh;
        MeshRenderer desMesh;
        if (!item.prefab.gameObject.TryGetComponent(out prefabMesh)) 
        {
            Debug.LogError("아이템의 프레펩에 MeshCollider가 존재하지 않습니다.");
            return false; 
        }
        if (!obj.TryGetComponent(out desMesh))
        {
            Debug.LogError("새로 생성한 그라운드아이템에 MeshRenderer가 존재하지 않습니다.");
            return false; 
        }
            
        desMesh.material = prefabMesh.sharedMaterial;
        return true;
    }
}