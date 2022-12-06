using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour
{
    Vector3 moveDelta = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // 가변프레임워크
    void Update()
    {
    }

    //고정프레임워크 ( 초당 몇번 반복할지 고정되어 있다. )
    private void FixedUpdate()
    {
        //물리엔진프레임과 동기화가 가능하다.
        transform.parent.Translate(moveDelta, Space.World);
        moveDelta = Vector3.zero;
    }
    private void OnAnimatorMove()
    {
        moveDelta += GetComponent<Animator>().deltaPosition;
        transform.parent.Rotate(GetComponent<Animator>().deltaRotation.eulerAngles, Space.World);
    }
}
