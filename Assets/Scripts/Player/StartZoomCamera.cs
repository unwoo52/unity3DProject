using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class StartZoomCamera : MonoBehaviour
{
    private PlayerScript playerinstance;
    private WaitForSeconds waitForSeconds = new(0.01f);
    private float waitFloat = 1f;
    public float deviedValue;
    public float zoomSpeed;

    //줌카메라의 원래 위치를 기억
    Vector3 originCamPos;
    Quaternion originCamRot;

    [SerializeField] private float disty;
    [SerializeField] private float distz;
    [SerializeField] private float rotatey;
    [SerializeField] private float rotatex;
    void Start()
    {
        originCamPos = this.transform.position;
        originCamRot = this.transform.rotation;
        playerinstance = PlayerScript.instance;

        //선언
        Vector3 Pos_tpCam = playerinstance.Com.tpCamera.transform.localPosition;
        Vector3 Pos_StartCam = transform.localPosition;

        Quaternion Quat_tpCam = playerinstance.Com.tpCamera.transform.localRotation;
        Quaternion Quat_StartCam = transform.localRotation;

        //프레임당 이동할 값 계산
        disty = (Pos_tpCam.y - Pos_StartCam.y) / deviedValue;
        distz = (Pos_tpCam.z - Pos_StartCam.z) / deviedValue;
        rotatey = (Quat_tpCam.eulerAngles.y - Quat_StartCam.eulerAngles.y) / deviedValue;
        rotatex = (Quat_tpCam.eulerAngles.x - Quat_StartCam.eulerAngles.x) / deviedValue;
    }
    private void Update()
    {
        StartCoroutine(CameraLogZoom());

    }
    IEnumerator CameraLogZoom()
    {
        while(deviedValue > 0)
        {
            waitFloat = Mathf.Lerp(waitFloat, 300f, Time.deltaTime * zoomSpeed);
            waitForSeconds = new(waitFloat);
            //logVal2 -= 0.1f;
            transform.localPosition += new Vector3(0, disty, 0);
            transform.localPosition += new Vector3(0, 0, distz);
            transform.Rotate(0, rotatey, 0);
            transform.Rotate(rotatex, 0, 0);
            deviedValue -= 1f;
            yield return waitForSeconds;
        }
        EndZoom();
    }
    private void EndZoom()
    {
        //위치 초기화
        this.transform.position = originCamPos;
        this.transform.Rotate(originCamRot.eulerAngles);
        //플레이어 STATE 변경
        playerinstance.ChangeState(PlayerScript.STATE.NORMAL);
        //SetActive False
        gameObject.SetActive(false);
    }
}
