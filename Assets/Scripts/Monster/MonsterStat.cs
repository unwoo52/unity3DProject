using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    [SerializeField] public MonsterData myData = default;

    [field: SerializeField] public float CurHp
    {
        get;
        private set;
    }
    [field: SerializeField] public float MoveSpeed
    {
        get;
        private set;
    }
    [field: SerializeField] public float RotSpeed
    {
        get;
        private set;
    }
    [field: SerializeField] public float AttackDelay
    {
        get;
        private set;
    }
    [field: SerializeField] public float AttackRange
    {
        get;
        private set;
    }
    [field: SerializeField] public float SkillAttackRange
    {
        get;
        private set;
    }
    [field: SerializeField] public float SkillAttackDelay
    {
        get;
        private set;
    }

    public float curAttackDelay;
    public float curSkillAttackDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        CurHp = myData.HP;
        MoveSpeed = myData.M_Speed;
        RotSpeed = myData.R_Speed;
        AttackDelay = myData.Delay;
        SkillAttackDelay = myData.SDelay;
        AttackRange = myData.Range;
        SkillAttackRange = myData.SkillRange;
    }   

    public void UpdateHP(float v)
    {
        CurHp = Mathf.Clamp(CurHp + v, 0.0f, myData.HP);
    }
    
}