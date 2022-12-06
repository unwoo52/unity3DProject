using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Monster, IBattle
{
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Battle:
                StopAllCoroutines();
                FollowTarget(mySensor.myTarget.transform, myStat.AttackRange, myStat.MoveSpeed, myStat.RotSpeed, OnAttack);
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
            case STATE.Battle:
                if (!myAnim.GetBool("IsAttacking")) myStat.curAttackDelay += Time.deltaTime;
                if (mySensor.myTarget != null && !mySensor.myTarget.IsLive)
                {
                    mySensor.OnLostTarget();
                }
                break;
        }
    }
    public void OnDamage(float dmg)
    {
        myHpBar.gameObject.SetActive(true);
        myStat.UpdateHP(-dmg);
        myHpBar.mySlider.value = myStat.CurHp / myStat.myData.HP;
        if (Mathf.Approximately(myStat.CurHp, 0.0f))
        {
            ChangeState(STATE.Death);
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateHpBar();
        mySensor.FindTarget += () => { if (Changerable()) ChangeState(STATE.Battle); };
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        if (mySensor.myTarget != null && Input.GetKeyDown(KeyCode.F2))
        {
            OnDamage(25.0f);
        }
    }
}

