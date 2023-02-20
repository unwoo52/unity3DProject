using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [SerializeField] private Mesh[] MeshList;
    [SerializeField] private Vector3[] ScaleList;
    [SerializeField] private GameObject DropItem;
    [SerializeField] private int curStoneSizeLevel;

    [SerializeField] private int stoneMaxSize;
    [SerializeField] private int numofDestroyDropItem;

    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;
    [SerializeField] private float maxCuttingHp;
    [SerializeField] private float cuttingHp;
    [SerializeField] private float maxCrackHp;
    [SerializeField] private float crackHp;
    //TestField
    public float testDmg;
    public float testPower;//튕겨져나가는 힘
    
    private void Start()
    {
        curHp = maxHp;
        curStoneSizeLevel = stoneMaxSize;
        cuttingHp = maxCuttingHp;
        crackHp = maxCrackHp;
    }
    void ChangeStoneSize(int i)
    {
        if (TryGetComponent<MeshFilter>(out var rockMeshFilter))
            rockMeshFilter.mesh = MeshList[i];
        if(TryGetComponent<MeshCollider>(out var rockMeshCollider))
            rockMeshCollider.sharedMesh = MeshList[i - 1];
        if(TryGetComponent<Transform>(out var rockTransform))
            rockTransform.localScale = ScaleList[i - 1];
    }
    public void DamToRock(Ray ray, Vector3 hit, float Dmg)
    {
        Dmg = testDmg; //TESTCODE===============================
        if (Dmg <= 0) Dmg = 0;
        cuttingHp -= Dmg;
        crackHp -= Dmg;
        if (crackHp <= 0)
        {
            int temp = CalculateTempHp(ref crackHp, ref maxCrackHp);
            while(temp > 0 && curHp > 0)
            {
                Dmg -= maxCrackHp;
                curHp -= maxCrackHp;
                instantiateDropItem(ray, hit);
                temp--;
            }
            curHp -= Dmg;
        }
        if(cuttingHp <= 0 && curHp > 0)
        {
            int temp = CalculateTempHp(ref cuttingHp, ref maxCuttingHp);
            curStoneSizeLevel -= temp;
            ChangeStoneSize(curStoneSizeLevel);
        }
        if (curHp <= 0) RockDestroyMethod();
    }
    private void instantiateDropItem(Ray ray, Vector3 hit)
    {
        GameObject obj = Instantiate(DropItem, hit, new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0));
        if (obj.TryGetComponent<Rigidbody>(out var rigid))
        {
            rigid.AddForce(-ray.direction * testPower, ForceMode.VelocityChange);
        }
        obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
    /// <summary>
    /// curTempHp보다 높은 데미지를 받아 curTempHp가 음수가 되었을 때
    /// maxTempHp의 몇배에 해당하는 데미지인지를 계산해 리턴
    /// </summary>
    /// <param name="curTempHp"></param>
    /// <param name="maxTempHp"></param>
    /// <returns>recover가 반복된 횟수</returns>
    private int CalculateTempHp(ref float curTempHp, ref float maxTempHp)
    {
        if (curTempHp > 0) return 0;
        int temp = 0;
        while(curTempHp <= 0)
        {
            curTempHp += maxTempHp;
            temp++;
        }
        return temp;
    }

    private void RockDestroyMethod()
    {
        if (TryGetComponent<MeshCollider>(out var mc)) Destroy(mc);
        if (TryGetComponent<MeshRenderer>(out var mr)) Destroy(mr);
        if (TryGetComponent<MeshFilter>(out var mf)) Destroy(mf);
        for (int i = 0; i < numofDestroyDropItem; i++)
        {
            Ray ray = new Ray(Vector3.zero, new Vector3(Random.Range(0, 300), Random.Range(0, 300), Random.Range(0, 300)));
            Vector3 hit = transform.position;
            instantiateDropItem(ray, hit);
        }
        //드롭템이 사라졌는지 1초마다 체크,
        //모두 사라졌다면 자기 자신 파괴
    }
}