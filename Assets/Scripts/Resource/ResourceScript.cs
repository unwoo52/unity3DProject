using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    //필드 ( Field )

    [SerializeField] float totalHP;
    [SerializeField] float curHP;

    //프로퍼티 ( Property )
    public float TotalHP
    {
        get => totalHP;
    }
    public float CurHP
    {
        get => curHP;
        set => curHP = Mathf.Clamp(value, 0.0f, totalHP);
    }
    public void OnDamageToResource(float dmg)
    {
        CurHP -= dmg;
        if (Mathf.Approximately(CurHP, 0.0f))
        {
            Destroy(gameObject);
            DropItem();
        }
        else
        {
            RenderingWhite();
            //myAnim.SetTrigger("Damage");
        }
        //myAnim.SetTrigger("OnHit");
    }
    public void DropItem()
    {
        GameObject itemGo = Instantiate(Resources.Load("Prefabs\\SM_Icon_Crafting_Rock_01")) as GameObject;
        itemGo.transform.position = this.gameObject.transform.position;
    }

    IEnumerator RenderingWhite()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
