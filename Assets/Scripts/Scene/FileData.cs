using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public struct PlayerData
{
    public List<string> Items;
    public float curHP;
    public Vector3 curPos;
}

public class FileData : MonoBehaviour
{
    public static FileData Inst = null;
    public PlayerData curData;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
    }
}
