﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moose : Monster, IBattle
{
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
                if (!mySensor.isActiveAndEnabled) mySensor.gameObject.SetActive(true);
                myAnim.SetBool("IsMoving", false);
                myAnim.SetBool("IsAttacking", false);
                StartCoroutine(GoingToRndPos());
                if(myHpBar != null) myHpBar.gameObject.SetActive(false);
                break;
            case STATE.Battle:
                StopAllCoroutines();
                mySensor.gameObject.SetActive(true);
                FollowTarget(mySensor.myTarget.transform, myStat.AttackRange, 2.0f * myStat.MoveSpeed, myStat.RotSpeed, OnAttack);
                myHpBar.gameObject.SetActive(true);
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
                if (mySensor.myTarget != null && !mySensor.myTarget.IsLive)
                {
                    mySensor.OnLostTarget();
                }
                if (Vector3.Distance(StartPos, transform.position) > 20.0f)
                {
                    ChangeState(STATE.Back);
                }
                break;
            case STATE.Back:
                mySensor.myTarget = null;
                if (!myAnim.GetBool("IsMoving")) ChangeState(STATE.Normal);
                break;
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
            myAnim.SetTrigger("Damage");
        }
    }
    #region 제자리로
    public void BackToPosition(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.position) > 20.0f)
        {
            StopAllCoroutines();
            transform.Translate(pos * myStat.MoveSpeed * 10.0f * Time.deltaTime);
        }
    }
    #endregion
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
        if (mySensor.myTarget != null && Input.GetKeyDown(KeyCode.F5))
        {
            OnDamage(25.0f);
        }

    }
}
