using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceStat
{
    //필드 ( Field )
    [SerializeField] float totalHP;
    [SerializeField] float curHP;

    //Gathering    Mining    Logging


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

    public ResourceStat(float hp)
    {
        curHP = totalHP = hp;
    }
}
