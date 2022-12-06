using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Monster Data", menuName = "Monster/Monster Data", order = -1)]

public class MonsterData : ScriptableObject
{
    [SerializeField] float MaxHp = 0.0f;
    [SerializeField] float MoveSpeed = 0.0f;
    [SerializeField] float rotSpeed = 0.0f;
    [SerializeField] float AttackDelay = 0.0f;
    [SerializeField] float SkillAttackDelay = 0.0f;
    [SerializeField] float AttackRange = 0.0f;
    [SerializeField] float SkillAttackRange = 0.0f;

    public float HP
    {
        get => MaxHp;
    }
    public float M_Speed
    {
        get => MoveSpeed;
    }
    public float R_Speed
    {
        get => rotSpeed;
    }
    public float Delay
    {
        get => AttackDelay;
    }
    public float SDelay
    {
        get => SkillAttackDelay;
    }
    public float Range
    {
        get => AttackRange;
    }
    public float SkillRange
    {
        get => SkillAttackRange;
    }
}
