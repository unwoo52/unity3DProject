using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Hare : Monster, IBattle 
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
                StartCoroutine(GoingToRndPos());
                break;
            case STATE.Runaway:
                StopAllCoroutines();
                RunAway(mySensor.myTarget.transform, 2.0f * myStat.MoveSpeed, myStat.RotSpeed);
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
            case STATE.Runaway:
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
    #region RunAway
    public void RunAway(Transform target, float MovSpeed, float RotSpeed)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(RunningAway(target, MovSpeed, RotSpeed));
        if (coRot != null) StopCoroutine(coRot);
    }
    IEnumerator RunningAway(Transform target, float MovSpeed, float RotSpeed)
    {
        while (target != null)
        {

            Vector3 dir = transform.position - target.position;
            dir.y = 0.0f;
            dir.Normalize();

            Vector3 rot = Vector3.RotateTowards(transform.forward, dir, RotSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rot);

            float delta = MovSpeed * Time.deltaTime;
            myAnim.SetTrigger("RunAway");
            transform.Translate(dir * delta, Space.World);

            yield return null;
        }
    }
    #endregion
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
            myAnim.SetTrigger("Damage");
        }
    }
    #region 제자리로
    //public void BackToPosition(Vector3 pos)
    //{
    //    if (Vector3.Distance(pos, transform.position) > 20.0f)
    //    {
    //        ChangeState(STATE.Back);
    //    }
    //}
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        ChangeState(STATE.Normal);
        CreateHpBar();
        mySensor.FindTarget += () => { if (Changerable()) ChangeState(STATE.Runaway); };
        mySensor.LostTarget += () => { if (Changerable()) ChangeState(STATE.Normal); };

    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        if (mySensor.myTarget != null && Input.GetKeyDown(KeyCode.F4))
        {
            OnDamage(25.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mySensor.myTarget != null)
        {
            ChangeState(STATE.Runaway);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (mySensor.myTarget == null)
        {
            ChangeState(STATE.Normal);
        }
    }
}
