using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerStat
    {
        //필드 ( Field )
        [SerializeField] float maxHp; //현재 체력 max
        [SerializeField] float curHP; //현재 체력 cur
        [SerializeField] float mydmg;
        public float curDelay;
        public float curAtDelay;

        public float TotalHP //체력 max를 반환
        {
            get => maxHp;
        }
        public float CurHP
        {
            get => curHP;
            set => curHP = Mathf.Clamp(value, 0.0f, maxHp);
        }

        public float myDmg
        {
            get => mydmg;
            set => mydmg = value;
        }

        //채광채집딜레이_줍기
        [SerializeField] float mineDelay_Picking_Origin;
        public float MineDelay_Picking_Origin { get => mineDelay_Picking_Origin; set { mineDelay_Picking_Origin = Mathf.Clamp(value, 1.0f, 10.0f); } }
        [SerializeField] float mineDelay_Picking_AfterBuff;
        public float MineDelay_Picking_AfterBuff { get => mineDelay_Picking_AfterBuff; set { mineDelay_Picking_AfterBuff = value; } }

        //채광채집딜레이_곡갱이질
        [SerializeField] float mineDelay_Mining_Origin;
        public float MineDelay_Mining_Origin { get => mineDelay_Mining_Origin; set { mineDelay_Mining_Origin = Mathf.Clamp(value, 1.0f, 10.0f); } }
        [SerializeField] float mineDelay_Mining_AfterBuff;
        public float MineDelay_Mining_AfterBuff { get => mineDelay_Mining_AfterBuff; set { mineDelay_Mining_AfterBuff = value; } }

        //채광데미지_곡갱이질
        [SerializeField] float mineDamage_Mining_Origin;
        public float MineDamage_Mining_Origin { get => mineDamage_Mining_Origin; set { mineDamage_Mining_Origin = Mathf.Clamp(value, 0f, 1000.0f); } }
        [SerializeField] float mineDamage_Mining_AfterBuff;
        public float MineDamage_Mining_AfterBuff { get => mineDamage_Mining_AfterBuff; set { mineDamage_Mining_AfterBuff = value; } }

        //채광채집딜레이_나무패기
        [SerializeField] float mineDelay_Logging_Origin;
        public float MineDelay_Logging_Origin { get => mineDelay_Logging_Origin; set { mineDelay_Logging_Origin = Mathf.Clamp(value, 1.0f, 10.0f); } }
        [SerializeField] float mineDelay_Logging_AfterBuff;
        public float MineDelay_Logging_AfterBuff { get => mineDelay_Logging_AfterBuff; set { mineDelay_Logging_AfterBuff = value; } }

        //전투딜레이_근접공격
        [SerializeField] float battleDelay_Melee_Origin;
        public float BattleDelay_Melee_Origin { get => battleDelay_Melee_Origin; set { battleDelay_Melee_Origin = Mathf.Clamp(value, 1.0f, 10.0f); } }
        [SerializeField] float battleDelay_Melee_AfterBuff;
        public float BattleDelay_Melee_AfterBuff { get => battleDelay_Melee_AfterBuff; set { battleDelay_Melee_AfterBuff = value; } }

        //사거리
        [SerializeField] float distancePicking = 10.0f;
        public float DistancePicking { get => distancePicking;}
        [SerializeField] float distanceMining = 10.0f;
        public float DistanceMining { get => distanceMining;}
        [SerializeField] float distanceAttacking = 10.0f;
        public float DistanceAttacking { get => distanceAttacking; }


        //이동속도
        [SerializeField] float moveSpeed_Origin;
        public float MoveSpeed_Origin { get => moveSpeed_Origin; set { moveSpeed_Origin = Mathf.Clamp(value, 0.1f, 100.0f); ; } }
        [SerializeField] float moveSpeed_Multiple;
        public float MoveSpeed__Multiple { get => moveSpeed_Multiple; set { moveSpeed_Multiple = value; } }
        [SerializeField] float moveSpeed_Plus;
        public float MoveSpeed_Plus { get => moveSpeed_Plus; set { moveSpeed_Plus = value; } }
        [SerializeField] float moveSpeed_AfterBuff;
        public float MoveSpeed_AfterBuff { get => moveSpeed_AfterBuff; set { moveSpeed_AfterBuff = Mathf.Clamp(value, 0.1f, 100.0f); } }

        //공격 데미지

        //채광 데미지
        [SerializeField] float powerMining_Origin = 10.0f;
        public float PowerMining_Origin { get => powerMining_Origin; }
        //채광 데미지
        [SerializeField] float powerLogging_Origin = 10.0f;
        public float PowerLogging_Origin { get => powerLogging_Origin; }


        public PlayerStat(float hp, float dmg, float moveSpeed_Origin, float mineDelay_Picking_Origin, float mineDelay_Mining_Origin, float mineDelay_Logging_Origin, float battleDelay_Melee_Origin)
        {
            curHP = maxHp = hp;
            mydmg = dmg;
            this.moveSpeed_Origin = moveSpeed_AfterBuff = moveSpeed_Origin;
            this.mineDelay_Picking_Origin = mineDelay_Picking_AfterBuff = mineDelay_Picking_Origin;
            this.mineDelay_Mining_Origin = mineDelay_Mining_AfterBuff = mineDelay_Mining_Origin;
            this.mineDelay_Logging_Origin = mineDelay_Logging_AfterBuff = mineDelay_Logging_Origin;
            this.battleDelay_Melee_Origin = battleDelay_Melee_AfterBuff = battleDelay_Melee_Origin;
            curDelay = 0.0f;
        }
    }
}