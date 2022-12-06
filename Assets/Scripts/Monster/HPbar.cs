using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

public class HpBar : MonoBehaviour
{
    public Transform myTarget;
    public Slider mySlider;
    PlayerScript playerinstance = PlayerScript.instance;


    // Update is called once per frame
    void Update()
    {
        if (playerinstance.State.isCurrentFp)
        {
            Vector3 pos = playerinstance.Com.fpCamera.WorldToScreenPoint(myTarget.position);
            if (pos.z < 0.0f)
            {
                pos.y = 1000.0f;
            }
            transform.position = pos;
        }
        else
        {
            Vector3 pos = playerinstance.Com.tpCamera.WorldToScreenPoint(myTarget.position);
            if (pos.z < 0.0f)
            {
                pos.y = 1000.0f;
            }
            transform.position = pos;
        }
    }
}
