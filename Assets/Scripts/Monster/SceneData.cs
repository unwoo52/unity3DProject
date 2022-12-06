using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public static SceneData Inst = null;
    public Canvas myCanvas = null;
    public Transform myHpBars;
    //public Camera myMinimapCamera = null;
    //public RectTransform myMinimap;
    //public RectTransform myPopups;
    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
