using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    //필드 ( Field )
    [SerializeField] float maxHp;
    [SerializeField] float curHP;

    //Gathering    Mining    Logging


    //프로퍼티 ( Property )
    public float TotalHP
    {
        get => maxHp;
    }
    public float CurHP
    {
        get => curHP;
        set => curHP = Mathf.Clamp(value, 0.0f, maxHp);
    }

    [SerializeField] float miningDelay_Gathering; //줍는 모션 딜레이
    public float MiningDelay_Gathering
    {
        get => miningDelay_Gathering;
    }
    //곡괭이질 모션 딜레이
    [SerializeField] float miningDelay_Mining;
    public float MiningDelay_Mining
    {
        get => MiningDelay_Mining;
    }
    public float MiningDelay_Mining_AfterBuffActivate;

    [SerializeField] float miningDelay_Logging; //나무 캐는 모션 딜레이
    public float MiningDelay_Logging
    {
        get => miningDelay_Logging;
        set => miningDelay_Logging = value;
    }



    [SerializeField] float rotSpeed;
    public float RotSpeed
    {
        get => rotSpeed;
    }
    [SerializeField] public float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
    }

    [SerializeField] float attackDelay;
    public float curAttackDelay;
    public float AttackDelay
    {
        get => attackDelay;
    }

    public CharacterStat(float hp, float moveSpeed, float rotSpeed, float attackDelay, float miningDelayLogging, float miningDelayGathering)
    {
        curHP = maxHp = hp;
        this.moveSpeed = moveSpeed;
        this.rotSpeed = rotSpeed;
        this.attackDelay = attackDelay;
        this.miningDelay_Logging = miningDelayLogging;
        this.miningDelay_Gathering = miningDelayGathering;
        curAttackDelay = 0.0f;
    }
}
