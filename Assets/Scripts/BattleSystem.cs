using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBattle
{
    Transform transform { get; }
    void OnDamage(float dmg);
    bool IsLive
    {
        get;
    }
}
public class BattleSystem : MonoBehaviour
{

}
