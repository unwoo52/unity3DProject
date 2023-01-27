using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour
{
    public ItemObject item;
    public GameObject groundItem;
    public LayerMask PlayerMask;

    private void Start()
    {
        //check grounditem
        if (this.groundItem == null) 
        {
            groundItem = GameObject.Find("Grounditem");
            if (groundItem != null) return;

            //if failed...
            Debug.LogError("아이템 드롭 실패"); 
            Destroy(gameObject); 
        }
    }

    private void DropItem(Collision collision)
    {
        

        GameObject obj;


        obj = Instantiate(this.groundItem, transform.position, new Quaternion(0, 0, 0, 0));
        obj.layer = this.groundItem.layer;
        if (obj.TryGetComponent(out GroundItem groundItemScript))
            groundItemScript.item = item;
        else
        {
            Debug.LogError("치명적 오류. GroundItem에 grountItemScript가 없습니다. 이 오류가 다른 모든 경우에서 발생할 수 있습니다.");
            Destroy(obj);
            return;
        }
    }


    //오브젝트 아이템과 플레이어 충돌시 버벅임을 없애기 위한
    //중력을 만든 코드.
    //오브젝트 아이템에 isTrigger을 활성화 하고, use grabity 비활성화
    //한 뒤 중력은 해당 코드로 처리하고자 함

    //중력을 적용하되, TerrainMask에 닿으면 멈추도록 함
    //TerrainMask : ground, ice, desert 등...

    /*
     private void ItemGrabity()
    {
        float delta = GrabitySpeed * Time.deltaTime;
        Ray ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, TerrainMasks))
        {
            if(hit.distance > hoverDistance)
            {
                if (hit.distance + hoverDistance < delta)
                {
                    delta = hit.distance + hoverDistance;
                };
                transform.Translate(Vector3.down * delta);
            }
        }
    }
     */

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & PlayerMask) != 0)
        {
            DropItem(collision);
            Destroy(gameObject);
        }
    }
}
