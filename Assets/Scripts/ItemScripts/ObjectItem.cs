using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour
{
    public ItemObject item;
    public GameObject groundItem;
    public LayerMask PlayerMask;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & PlayerMask) != 0)
        {
            GameObject obj = Instantiate(this.groundItem, transform.position, new Quaternion(0,0,0,0));
            obj.layer = this.groundItem.layer;
            if (obj.TryGetComponent(out GroundItem groundItem))
                groundItem.item = item;
            Destroy(gameObject);
        }
    }
}
