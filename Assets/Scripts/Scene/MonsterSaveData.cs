using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyData
{
    public float curHP;
    public Vector3 curPos;
}

[Serializable]
public class EnemySaveList
{
    public List<EnemyData> EnemyList = new List<EnemyData>();
}


public class MonsterSaveData : MonoBehaviour
{
    public MonsterSaveData Inst = null;
    public EnemySaveList saveData = new EnemySaveList();

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
    }
}
