using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityEvent RHandAttack = null;
    public UnityEvent TwoHandedAttack = null;
    public UnityEvent OneHandedAttack = null;
    public UnityEvent BowAttack = null;
    public UnityEvent Mining = null;
    public UnityEvent TreeGet = null;
    public UnityEvent Gethering = null;
    public UnityEvent Attack = null;    // 몬스터용
    public UnityEvent SkAttack = null;  // 몬스터용


    public void AttackAnimEvent()
    {
        RHandAttack?.Invoke();
    }

    public void BowAttackAnimEvent()
    {
        BowAttack?.Invoke();
    }

    public void TwoHandedAttackAnimEvent()
    {
        TwoHandedAttack?.Invoke();
    }

    public void OneHandedAttackAnimEvent()
    {
        OneHandedAttack?.Invoke();
    }

    public void MiningAnimEvent()
    {
        Mining?.Invoke();
    }
    public void MiningTreeEvent()
    {
        TreeGet?.Invoke();
    }
    public void GetheringAnimEvent()
    {
        Gethering?.Invoke();
    }

    public void testScript()
    {
        Debug.Log("Test");
    }
    public void OnAttack() // 몬스터용
    {
        Attack?.Invoke();
    }
    public void OnSkillAttackEvent() // 몬스터용
    {
        SkAttack?.Invoke();
    }
}
