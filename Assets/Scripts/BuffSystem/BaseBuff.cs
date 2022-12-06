using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

/*
 * BaseBuff에 대한 설명
 * 버프에 대한 정보를 init으로 type, value, time, image을 받아 실행
 * playerScript의 buff list에 버프를 추가(BuffListAdd) 및 버프 효과 갱신(ChooseBuff)를 실행
 * 버프가 지속시간이 다 되면 자신을 파괴함과 동시에 playerScirpt의 buff list에서 다시 자신의 인스턴스를 삭제(removeBuff(this.BaseBuff)) 및 버프 효과 갱신(ChooseBuff)을 실행
 */
public class BaseBuff : MonoBehaviour //BuffPrefeb의 스크립트
{
    #region Buff Data
    public IBuff PlayerIBuff;

    //1. 버프의 효과 종류 이름(공격력 디버프, 방어력 디버프)을 받아올 변수 ex)디버프오라 "방어력"
    List<string> buffTypenameList = new();
    public List<string> BuffTypenameList { get => buffTypenameList; }

    //2-1. 얼마큼의 값을 변화시켜줄지 ex)디버프오라 방어력 "-10%"
    List<float> buffValueList = new();
    public List<float> BuffValueList { get => buffValueList; }

    //3. 버프 지속시간 ex)유니온오라 "40초"
    float buffOriginTime;
    public float BuffOriginTime { get => buffOriginTime; }

    //4. 버프 아이콘
    public Image icon; //여기에 버프 이미지 보관 ex)"디버프오라 아이콘"

    //5. 버프 현재 남은 시간. root method에서 지속적으로 deltatime을 뺌
    float currentTime;

    //6. TerrainLayerIntNumber을 갖고있거나 ex)31 = ice, 그 외의 버프 코드를 저장
    int buffCode;
    public int BuffCode { get => buffCode; }
    
    //7. 나중에 deltaTime으로 바꿔야 함
    readonly WaitForSeconds BuffCheckRootSecond = new(0.1f);

    //8. 가중치 타임
    public float WeightTime;

    #endregion
    private void Awake()
    {
        icon = GetComponent<Image>();
    }
    /// <summary>
    /// 일반 버프
    /// </summary>
    /// <param name="buffTypenameList"></param>
    /// <param name="buffValueList"></param>
    public void Init(List<string> buffTypenameList, List<float> buffValueList) //받아온 정보를 이 prefep에 init
    {
        this.buffTypenameList = buffTypenameList;
        this.buffValueList = buffValueList;
        currentTime = this.buffOriginTime;
        icon.fillAmount = 1f;

        PlayerIBuff = PlayerScript.instance.GetComponent<IBuff>();
        Destroy(gameObject.GetComponent<Button>());
        NonCoroutineBuffActivation();
    }
    /// <summary>
    /// 일반 버프
    /// </summary>
    /// <param name="buffTypenameList"></param>
    /// <param name="buffValueList"></param>
    /// <param name="buffOriginTime"></param>
    public void Init(List<string> buffTypenameList, List<float> buffValueList, float buffOriginTime) //받아온 정보를 이 prefep에 init
    {
        this.buffTypenameList = buffTypenameList;
        this.buffValueList = buffValueList;
        this.buffOriginTime = buffOriginTime;
        currentTime = this.buffOriginTime;
        icon.fillAmount = 1f;

        PlayerIBuff = PlayerScript.instance.GetComponent<IBuff>();
        NomalBuffactivation();
    }
    /// <summary>
    /// Terrain 버프
    /// </summary>
    /// <param name="buffTypenameList"></param>
    /// <param name="buffValueList"></param>
    /// <param name="buffcode">지형 레이어 번호</param>
    public void Init(List<string> buffTypenameList, List<float> buffValueList, int buffcode)
    {
        this.buffTypenameList = buffTypenameList;
        this.buffValueList = buffValueList;
        buffCode = buffcode;
        icon.fillAmount = 1f;

        PlayerIBuff = PlayerScript.instance.GetComponent<IBuff>();
        NonCoroutineBuffActivation();
    }
    /// <summary>
    /// 가중치 버프
    /// </summary>
    /// <param name="buffTypenameList"></param>
    /// <param name="buffValueList"></param>
    /// <param name="buffcode"></param>
    /// <param name="s">가중치 효과 이름</param>
    /// <param name="value">가중치 값</param>
    /// <param name="WeightTime">가중치 시간</param>
    public void Init(List<string> buffTypenameList, List<float> buffValueList, string s, float value, float WeightTime)
    {
        this.buffTypenameList = buffTypenameList;
        this.buffValueList = buffValueList;
        this.WeightTime = WeightTime;
        currentTime = buffOriginTime;
        icon.fillAmount = 1f;


        PlayerIBuff = PlayerScript.instance.GetComponent<IBuff>();
        NonCoroutineBuffActivation();
        StartCoroutine(WeightStatAdd(s, value));
        Destroy(gameObject.GetComponent<Button>());
    }


    #region 버프가 생성될 때 버프효과 적용
    void NonCoroutineBuffActivation()
    {
        PlayerIBuff.BuffListAdd(this);
        PlayerIBuff.ChooseBuff(buffTypenameList);
    }
    private void NomalBuffactivation()
    {
        PlayerIBuff.BuffListAdd(this);
        PlayerIBuff.ChooseBuff(buffTypenameList);
        StartCoroutine(Activation());
    }
    #endregion

    #region 버프효과 Root
    //타이머
    IEnumerator Activation()
    {
        while (currentTime > 0)
        {
            icon.fillAmount = currentTime / buffOriginTime;
            //Buff Root
            currentTime -= 0.1f;
            yield return BuffCheckRootSecond;
        }
        icon.fillAmount = 0f;
        currentTime = 0f;
        BuffDeActivation();
    }

    IEnumerator WeightStatAdd(string s, float value)
    {
        while (this.gameObject != null)
        {
            PlayerIBuff.BuffValueApply(s, value);
            yield return new WaitForSeconds(WeightTime);
        }
    }

    IEnumerator WeightBuffAdd(int i, float value)
    {
        while (this.gameObject != null)
        {
            BuffTypenameList[i] += value;
            yield return new WaitForSeconds(WeightTime);
        }
    }
    #endregion

    #region 버프효과 파괴
    public void BuffDeActivation() // 버프 파괴 메서드
    {
        PlayerIBuff.RemovBuff(this);
        PlayerIBuff.ChooseBuff(buffTypenameList);
        Destroy(this.gameObject);
    }
    #endregion

    #region Click Method
    public void ClickBuffDeActivation()
    {
        BuffDeActivation();
    }
    #endregion
}
