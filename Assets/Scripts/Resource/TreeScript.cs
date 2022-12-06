using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField] GameObject TrunkBottom;
    [SerializeField] GameObject TrunkCylinder;
    [SerializeField] GameObject Bush1;
    [SerializeField] GameObject Leaf;

    [SerializeField] int numberLeaf;
    [SerializeField] int numberBush;

    [SerializeField] int numberTreeEffect;
    [SerializeField] float distanceGapofCylinder;

    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;

    public Vector3 positionLeaf;
    public Vector3 positionTrunkBottom;
    public Vector3 positionTrunkCylinder;

    //TestField 
    public float RotationValue;

    private GameObject[] leafList;
    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
    }

    public void DamtoTree(float f)
    {
        curHp -= f;
        if (curHp == 0) TreeDestroy();
    }

    void TreeDestroy()
    {
        leafList = new GameObject[(numberBush + numberLeaf) * numberTreeEffect];
        if (TryGetComponent<CapsuleCollider>(out var cc)) Destroy(cc);
        if (TryGetComponent<MeshRenderer>(out var mr)) Destroy(mr);
        if (TryGetComponent<MeshFilter>(out var mf)) Destroy(mf);
        TreeDestroyEffect();
    }

    void TreeDestroyEffect()
    {
        Instantiate(TrunkBottom, transform.position + positionTrunkBottom, transform.rotation);
        for (int i = 0; i < numberTreeEffect; i++)
        {
            InstanteTreeEffect(transform.position + new Vector3(0, i * distanceGapofCylinder, 0) , i);
        }
        StartCoroutine(CleaningBushandLeaf());
    }

    void InstanteTreeEffect(Vector3 position, int i)
    {
        Instantiate(TrunkCylinder, position + positionTrunkCylinder, TrunkCylinder.transform.rotation);
        int tempNumberBush = numberBush;
        int tempNumberLeaf = numberLeaf;

        while (tempNumberBush > 0)
        {
            GameObject obj = Instantiate(Bush1,
                position + positionLeaf + new Vector3(Random.Range(-3, 3), Random.Range(0, 3), Random.Range(-3, 3)),
                new Quaternion(Random.Range(0, RotationValue), Random.Range(0, RotationValue), Random.Range(0, RotationValue), 0));
            leafList[(numberBush + numberLeaf) * i + numberLeaf + tempNumberBush - 1] = obj;
            tempNumberBush--;
        }
        while (tempNumberLeaf > 0)
        {
            GameObject obj = Instantiate(Leaf,
                position + positionLeaf + new Vector3(Random.Range(-3, 3), Random.Range(0, 3), Random.Range(-3, 3)),
                new Quaternion(Random.Range(0, RotationValue), Random.Range(0, RotationValue), Random.Range(0, RotationValue), 0));
            leafList[(numberBush + numberLeaf) * i + tempNumberLeaf - 1] = obj;
            tempNumberLeaf--;
        }
    }
    // 땅에 떨어진 잎과 풀을 일정 시간 이후에 모두 제거
    IEnumerator CleaningBushandLeaf()
    {
        yield return new WaitForSeconds(5);
        foreach(GameObject obj in leafList)
        {
            Destroy(obj.gameObject);
            //or obj.FadeLeaf();
        }
    }
}
