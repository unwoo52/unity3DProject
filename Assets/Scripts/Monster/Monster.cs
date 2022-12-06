using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void MyAction();
public delegate void MyAction<T>(T t);
public class Monster : MonoBehaviour
{
    public STATE myState = STATE.Create;
    public Transform myHeadPos;
    public Coroutine coMove = null;
    public Coroutine coRot = null;
    public MonsterStat myStat;
    public AIPerception mySensor = null;
    protected Vector3 StartPos = Vector3.zero;
    protected HpBar myHpBar = null;
    public Renderer myRenderer;


    #region 프로퍼티
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                if (_anim == null)
                {
                    _anim = GetComponentInChildren<Animator>();
                }
            }
            return _anim;
        }
    }
    Rigidbody _rigidbody = null;
    protected Rigidbody myrigidbody
    {
        get
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
                if (_rigidbody == null)
                {
                    _rigidbody = GetComponentInParent<Rigidbody>();
                }
            }
            return _rigidbody;
        }
    }
    #endregion

    public enum STATE
    {
        Create, Normal, Battle, Runaway, Back, SkAttack, Death
    }

    #region 전투시스템
    public Transform HeadPos
    {
        get => myHeadPos;
    }
    public bool IsLive
    {
        get
        {
            if (Mathf.Approximately(myStat.CurHp, 0.0f))
            {
                return false;
            }
            return true;
        }
    }
    #endregion

    protected IEnumerator GoingToRndPos()
    {
        yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
        Vector3 rndPos = StartPos;
        rndPos.x += Random.Range(-5.0f, 5.0f);
        rndPos.z += Random.Range(-5.0f, 5.0f);
        MoveToPosition(rndPos, myStat.MoveSpeed, myStat.RotSpeed, () => StartCoroutine(GoingToRndPos()));
    }
    protected void BackToPosition(Vector3 targetPos, float MovSpeed = 1.0f, float RotSpeed = 360.0f, MyAction done = null, float distance = 0.0f)
    {
        if (Vector3.Distance(targetPos, transform.position) < 0.01f)
        {
            done?.Invoke();
            return;
        }
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(MovingToPosition(targetPos, MovSpeed, distance, done));
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform, targetPos, RotSpeed));
    }

    public void OnAttack()
    {
        if (mySensor.myTarget != null && !myAnim.GetBool("IsAttacking"))
        {
            if (myStat.curAttackDelay >= myStat.AttackDelay)
            {
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger("Attack");
                myStat.curAttackDelay = 0.0f;
            }
        }
        myAnim.SetBool("IsAttacking", false);
    }
    public void AttackTarget()
    {
        if (mySensor.myTarget != null)
        {
            if (mySensor.myTarget.IsLive) mySensor.myTarget.OnDamage(30.0f);
            Debug.Log("<color=yellow>피격</color>");
        }
    }

    #region MovePosition
    protected void MoveToPosition(Vector3 targetPos, float MovSpeed = 1.0f, float RotSpeed = 360.0f, MyAction done = null, float distance = 0.0f)
    {
        if (Vector3.Distance(targetPos, transform.position) < 0.01f)
        {
            done?.Invoke();
            return;
        }
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(MovingToPosition(targetPos, MovSpeed, distance, done));
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform, targetPos, RotSpeed));
    }

    IEnumerator MovingToPosition(Vector3 target, float MovSpeed, float distance, MyAction done = null)
    {
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        if (dist <= Mathf.Epsilon) yield break;
        dir.Normalize();

        myAnim.SetBool("IsMoving", true);

        while (dist > distance)
        {
            if (!myAnim.GetBool("IsAttacking"))
            {
                float delta = MovSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                break;
            }
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }
    #endregion
    public static IEnumerator Rotating(Transform transform, Vector3 target, float RotSpeed)
    {
        Vector3 dir = target - transform.position;
        if (dir.magnitude <= Mathf.Epsilon) yield break;
        dir.Normalize();
        float d = Vector3.Dot(dir, transform.forward);
        float r = Mathf.Acos(d);
        float angle = r * Mathf.Rad2Deg;

        if (angle > Mathf.Epsilon)
        {
            float rotDir = Vector3.Dot(dir, transform.right) < 0.0f ? -1.0f : 1.0f;
            while (angle > Mathf.Epsilon)
            {
                float delta = RotSpeed * Time.deltaTime;
                if (delta > angle)
                {
                    delta = angle;
                }
                angle -= delta;
                transform.Rotate(delta * rotDir * Vector3.up, Space.World);
                yield return null;
            }
        }
    }
    #region FollowTarget
    protected void FollowTarget(Transform target, float AttackRange, float MovSpeed, float RotSpeed, MyAction reached = null)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(FollowingTarget(target, AttackRange, MovSpeed, RotSpeed, reached));
        if (coRot != null) StopCoroutine(coRot);
    }
    IEnumerator FollowingTarget(Transform target, float AttackRange, float MovSpeed, float RotSpeed, MyAction reached)
    {
        while (target != null)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0.0f;
            float dist = dir.magnitude;

            Vector3 rot = Vector3.RotateTowards(transform.forward, dir, RotSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rot);

            if (!myAnim.GetBool("IsAttacking") && !myAnim.GetBool("IsAttacked")  && dist > AttackRange + 0.01f)
            {
                myAnim.SetBool("IsMoving", true);
                dir.Normalize();
                float delta = MovSpeed * Time.deltaTime;
                if (delta > dist - AttackRange)
                {
                    delta = dist - AttackRange;
                    myAnim.SetBool("IsMoving", false);
                }
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                myAnim.SetBool("IsMoving", false);
                reached?.Invoke();
            }
            yield return null;
        }
    }
    #endregion
    
    public bool Changerable()
    {
        return myState != STATE.Death;
    }
    #region HpBar
    public void CreateHpBar()
    {
        if (myHpBar == null)
        {
            GameObject obj = Instantiate(Resources.Load("Prefabs/HpBar")) as GameObject;
            myHpBar = obj.GetComponent<HpBar>();
            myHpBar.myTarget = myHeadPos;
            obj.transform.SetParent(SceneData.Inst.myHpBars);
        }
    }
    public void DeleteHpBar()
    {
        if (myHpBar != null) Destroy(myHpBar.gameObject);
    }
    #endregion

    #region DisAppear
    public void DisAppear()
    {
        StartCoroutine(DisAppearing());
    }
    IEnumerator DisAppearing()
    {
        yield return new WaitForSeconds(3.0f);
        Material mat = myRenderer.material;
        myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        while (mat.GetFloat("_DissolveAmount") < 1.0f)
        {
            mat.SetFloat(("_DissolveAmount"), mat.GetFloat("_DissolveAmount") + (0.5f * Time.deltaTime));
            yield return null;
        }
        Destroy(gameObject);
    }
    #endregion
}
