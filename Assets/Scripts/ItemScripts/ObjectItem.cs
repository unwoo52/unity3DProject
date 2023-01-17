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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & PlayerMask) != 0)
        {
            DropItem(collision);
            Destroy(gameObject);
        }
    }
}
