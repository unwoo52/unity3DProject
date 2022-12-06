using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeer : Monster, IBattle
{
    public GameObject myRange;
    public SkillRange mySkillRange;
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Normal:
                StopAllCoroutines();
                myRange.SetActive(false);
                mySensor.gameObject.SetActive(true);
                myAnim.SetBool("IsMoving", false);
                myAnim.SetBool("IsAttacking", false);
                if (myHpBar != null) myHpBar.gameObject.SetActive(false);
                break;
            case STATE.Battle:
                StopAllCoroutines();
                myRange.SetActive(false);
                myHpBar.gameObject.SetActive(true);
                FollowTarget(mySensor.myTarget.transform, myStat.AttackRange, 2.0f * myStat.MoveSpeed, myStat.RotSpeed, OnAttack);
                break;
            case STATE.SkAttack:
                
                StopAllCoroutines();
                OnSkillAttack();
                break;
            case STATE.Back:
                StopAllCoroutines();
                mySensor.gameObject.SetActive(false);
                myAnim.SetBool("IsMoving", false);
                BackToPosition(StartPos, 5.0f * myStat.MoveSpeed, myStat.RotSpeed);
                break;
            case STATE.Death:
                DeleteHpBar();
                StopAllCoroutines();
                myRange.SetActive(false);
                myAnim.SetTrigger("Death");
                mySensor.enabled = false;
                GetComponent<Collider>().enabled = false;
                DisAppear();
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Normal:
                break;
            case STATE.Battle:
                if (!myAnim.GetBool("IsAttacking")) myStat.curAttackDelay += Time.deltaTime;
                if (!myAnim.GetBool("IsSkill")) myStat.curSkillAttackDelay += Time.deltaTime;
                if (mySensor.myTarget != null && !mySensor.myTarget.IsLive)
                {
                    mySensor.OnLostTarget();
                    ChangeState(STATE.Normal);
                }
                if (Vector3.Distance(StartPos, transform.position) > 20.0f)
                {
                    ChangeState(STATE.Back);
                }
                if (myStat.curSkillAttackDelay >= myStat.SkillAttackDelay)
                {
                    ChangeState(STATE.SkAttack);
                }
                break;
            case STATE.SkAttack:
                if (!myAnim.GetBool("IsAttacking")) myStat.curAttackDelay += Time.deltaTime;
                if (myStat.curAttackDelay > 1.5f)
                {
                    ChangeState(STATE.Battle);
                }
                break;
            case STATE.Back:
                mySensor.myTarget = null;
                if (!myAnim.GetBool("IsMoving")) ChangeState(STATE.Normal);
                break;
        }
    }
     
    public void OnSkillAttack()
    {
        if (!myAnim.GetBool("IsSkill") && !myAnim.GetBool("IsAttacking") && mySensor.myTarget != null)
        {
            myAnim.SetBool("IsSkill", true);
            if (myStat.curSkillAttackDelay >= myStat.SkillAttackDelay 
                && Vector3.Distance(mySensor.myTarget.transform.position, transform.position) < myStat.SkillAttackRange)
            {
                myAnim.SetTrigger("SkillAttack");
            }
        }
        myRange.SetActive(true);
        myAnim.SetBool("IsSkill", false);
        
        myStat.curSkillAttackDelay = 0.0f;
        myStat.curAttackDelay = 0.0f;
       
    }
    public void SkillAttackTarget()
    {
        if (mySensor.myTarget != null)
        {
            if (mySensor.myTarget.IsLive)
            {
                if (mySkillRange.checkMyTarget != null) 
                {
                    mySkillRange._myTarget.OnDamage(50.0f);
                    Debug.Log("<color=red>스킬 피격</color>");
                }
                else
                {
                    Debug.Log("<color=green>스킬 회피</color>");
                }
            }
        }
    }

    public void OnDamage(float dmg)
    {
        myStat.UpdateHP(-dmg);
        myHpBar.mySlider.value = myStat.CurHp / myStat.myData.HP;
        if (Mathf.Approximately(myStat.CurHp, 0.0f))
        {
            ChangeState(STATE.Death);
        }
        else
        {
            ChangeState(STATE.Battle);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        CreateHpBar();
        ChangeState(STATE.Normal);
        mySensor.LostTarget += () => { if (Changerable()) ChangeState(STATE.Normal); };
        
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnDamage(30.0f);
        }
    }
    
}
