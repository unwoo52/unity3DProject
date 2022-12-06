using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBuff
{
    //BuffList(BaseBuff)를 선언해 사용
    /// <summary>
    /// player의 BuffList에 버프 인스턴스를 추가하는 메소드
    /// </summary>
    /// <param name="baseBuff">추가할 인스턴스</param>
    void BuffListAdd(BaseBuff baseBuff);
    /// <summary>
    /// player의 BuffList에서 버프 인스턴스를 삭제하는 메소드
    /// </summary>
    /// <param name="baseBuff">삭제할 인스턴스</param>
    void RemovBuff(BaseBuff baseBuff);
    /// <summary>
    /// buff Type name이 저장된 리스트를 전달하여 player의 해당되는 모든 buff type의 stat을 갱신하는 메소드
    /// foreach로 리스트의 각 인자에 접근하고, switch문으로 인자와 일치하는 buff type name의 stat를 갱신하는 BuffEffectApply를 실행
    /// </summary>
    /// <param name="buffTypenameList">버프 인스턴스의 buffTypenameList</param>
    void ChooseBuff(List<string> buffTypenameList);
    /// <summary>
    /// 버프 갱신 메소드
    /// 플레이어의 버프 리스트를 모두 읽어들여 s가 일치하는 element를 탐색, 해당 element에 대응되는 buff value를 temp에 더한 후, temp + origin을 return
    /// </summary>
    /// <param name="s">적용할 버프 타입</param>
    /// <param name="origin">적용할 버프 타입의 origin</param>
    /// <returns></returns>
    float BuffEffectAplly(string s, float origin);
    /// <summary>
    /// 플레이어의 스탯에 버프 값을 더함
    /// </summary>
    /// <param name="s">Value 이름 ex)hp</param>
    /// <param name="value">Value 값 ex)10</param>
    void BuffValueApply(string s, float value);

}
/*
 * BuffManagerScript에 대한 설명
 * 이 스크립트는 buffObject 프리펩을 가지며, 이 프리펩을 instantiate하여 버프를 생성하고 baseBuff.init()을 실행한다
 * 생성된 프리펩을 BuffManager Script가 위치한 UI Panel에 buffObject 프리펩을 보관한다
 */
public class BuffManagerScript : MonoBehaviour //UI중 Buff Panel에 인스턴스
{
    #region instance
    public static BuffManagerScript instance;
    [SerializeField] private GameObject buffPrefab;
    private void Awake()
    {
        instance = this;
        buffPrefab = Resources.Load("Prefabs/buffObject") as GameObject;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffTypename">버프 효과 이름</param>
    /// <param name="buffValue">버프 효과 수치</param>
    /// <param name="buffOriginTime">지속시간</param>
    /// <param name="bufficon">Sprite 버프 아이콘</param>
    /// 
    public void CreateBuff(List<string> buffTypename, List<float> buffValue, Sprite bufficon)
    {
        GameObject gameObject = Instantiate(buffPrefab, transform); //인스턴스 생성(buffBase), 위치는 this(MyBuffPanel)에.
        //gameobject = instanced baseBuff prefep
        gameObject.GetComponent<BaseBuff>().Init(buffTypename, buffValue); //인스턴스에 버프 정보 입력
        gameObject.GetComponent<Image>().sprite = bufficon; //
    }
    public void CreateBuff(List<string> buffTypename, List<float> buffValue, float buffOriginTime, Sprite bufficon)
    {
        GameObject gameObject = Instantiate(buffPrefab, transform); //인스턴스 생성(buffBase), 위치는 this(MyBuffPanel)에.
        //gameobject = instanced baseBuff prefep
        gameObject.GetComponent<BaseBuff>().Init(buffTypename, buffValue, buffOriginTime); //인스턴스에 버프 정보 입력
        gameObject.GetComponent<Image>().sprite = bufficon; //
    }
    public void CreateBuff(List<string> buffTypename, List<float> buffValue, Sprite bufficon, int buffcode)
    {
        GameObject gameObject = Instantiate(buffPrefab, transform); //인스턴스 생성(buffBase), 위치는 this(MyBuffPanel)에.
        //gameobject = instanced baseBuff prefep
        gameObject.GetComponent<BaseBuff>().Init(buffTypename, buffValue, buffcode); //인스턴스에 버프 정보 입력
        gameObject.GetComponent<Image>().sprite = bufficon; //
        if (29 <= buffcode || buffcode <= 31) Destroy(gameObject.GetComponent<Button>());
    }
    public void CreateBuff(List<string> buffTypename, List<float> buffValue, Sprite bufficon, string s, float value, float WeightTime)
    {
        GameObject gameObject = Instantiate(buffPrefab, transform); //인스턴스 생성(buffBase), 위치는 this(MyBuffPanel)에.
        //gameobject = instanced baseBuff prefep
        gameObject.GetComponent<BaseBuff>().Init(buffTypename, buffValue, s, value, WeightTime); //인스턴스에 버프 정보 입력
        gameObject.GetComponent<Image>().sprite = bufficon;
    }
}
