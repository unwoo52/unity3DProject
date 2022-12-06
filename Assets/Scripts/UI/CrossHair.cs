using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    #region singletone
    private static CrossHair crosshairInstance = null;

    void Awake()
    {
        if (crosshairInstance == null)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            crosshairInstance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }
    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static CrossHair CrosshairInstance
    {
        get
        {
            if (null == crosshairInstance)
            {
                return null;
            }
            return crosshairInstance;
        }
    }
    #endregion

    #region field
    private Canvas canvas;
    private RawImage rawImage;
    private float widthLength;
    private float heightLength;
    [SerializeField] private bool isCrosshiarOn;
    [SerializeField] private Texture[] CrosshiarArray;
    #endregion

    void Start()
    {
        canvas = GetCameraComponent();
        rawImage = GetRawImageComponent();


        ChangeCrossHair(0);
        GetCrosshiarScale();
        AlignCrosshair();
    }
    [ContextMenu("ChangeCrosshairState")]
    private void ChangeCrosshairState()
    {
        //if (isCrosshiarOn) rawImage.color.a = 255f;
        //else color.a = 0f;
    }

    public void ChangeCrossHair(int i)
    {
        rawImage.texture = CrosshiarArray[i];
    }

    #region method
    private void GetCrosshiarScale()
    {
        if (TryGetComponent(out RectTransform rectTransform))
        {
            widthLength = rectTransform.rect.width;
            heightLength = rectTransform.rect.height;
        }
        else Debug.LogError("RectTransform 컴포넌트가 존재하지 않습니다");
    }
    private void AlignCrosshair()
    {
        transform.position = GetMidMonitor() - new Vector3(widthLength / 2, heightLength / 2, 0);
    }

    public Vector3 GetMidMonitor()
    {
        if(canvas == null)
        {
            Debug.LogError("카메라를 받아오지 않았습니다. 먼저 카메라를 받아오십시오");
            return Vector3.zero;
        }
        return new Vector3(canvas.pixelRect.width / 2, canvas.pixelRect.height / 2);
    }
    private RawImage GetRawImageComponent()
    {
        if (TryGetComponent(out RawImage rawImage))
        {
            return rawImage;
        }
        else
        {
            Debug.LogError("RawImage 컴포넌트가 존재하지 않습니다. 인스펙터창을 확인하십시오");
            return null;
        }
    }
    private Canvas GetCameraComponent()
    {
        if (transform.parent.TryGetComponent(out Canvas canvas))
        {
            return canvas;
        }
        else
        {
            Debug.LogError("부모 오브젝트가 캔버스가 아닙니다. 부모 오브젝트를 확인하십시오");
            return null;
        }
    }
    #endregion
}
