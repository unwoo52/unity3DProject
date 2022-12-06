using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    public MeshFilter myFilter;
    public float ViewAngle = 90.0f;
    public float ViewDistance = 10.0f;
    public int DetailCount = 100;   // 부채살 갯수
    Vector3[] myDirs = null;
    public LayerMask EnemyMask = default;
    public GameObject checkMyTarget;
    public IBattle _myTarget;

    //MeshCollider meshCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vb = new Vector3[DetailCount + 1];    // 꼭지점 Vertex Buffer
        int[] ib = new int[(DetailCount - 1) * 3];  // 인덱스버퍼 Index Buffer
        myDirs = new Vector3[DetailCount];      // 벡터 배열
        Vector3 dir = transform.forward * ViewDistance;     // 바라보는 방향 * 부채꼴의 길이
        //쿼터니언 * 벡터 = 회전된 벡터
        myDirs[0] = Quaternion.AngleAxis(-ViewAngle / 2.0f, Vector3.up) * dir;

        float angleGap = ViewAngle / (float)(DetailCount - 1);  // 부채살 사이의 각도

        vb[0] = Vector3.zero;
        for (int i = 1; i < vb.Length; ++i)
        {
            vb[i] = vb[0] + myDirs[i - 1];
            if (i < DetailCount) myDirs[i] = Quaternion.AngleAxis(angleGap, Vector3.up) * myDirs[i - 1];

            if (i >= 2)
            {
                ib[(i - 2) * 3] = 0;
                ib[(i - 2) * 3 + 1] = i - 1;
                ib[(i - 2) * 3 + 2] = i;
            }
        }

        Mesh _mesh = new Mesh();
        
        _mesh.vertices = vb;
        _mesh.triangles = ib;
        myFilter.mesh = _mesh;

        //meshCollider = gameObject.AddComponent<MeshCollider>();
        //meshCollider.convex = true;
        //meshCollider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkMyTarget = null;
        Vector3[] vb = myFilter.mesh.vertices;
        for (int i = 0; i < myDirs.Length; ++i)
        {
            if (Physics.Raycast(transform.position, transform.rotation * myDirs[i], out RaycastHit hit, ViewDistance, EnemyMask))
            {
                vb[i + 1] = vb[0] + myDirs[i].normalized * ViewDistance;
                if ((EnemyMask & 1 << hit.transform.gameObject.layer) != 0)
                {
                    checkMyTarget = hit.transform.gameObject;
                    _myTarget = checkMyTarget.gameObject.GetComponentInParent<IBattle>();
                }
                else
                {
                    checkMyTarget = null;
                }
            }
            else
            {
                vb[i + 1] = vb[0] + myDirs[i].normalized * ViewDistance;
            }
        }
        myFilter.mesh.vertices = vb;
    }

    
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        Debug.Log("충돌");
    //        IBattle ib = collision.transform.GetComponent<IBattle>();
    //        ib?.OnDamage(50.0f);
    //    }
    //}
}
