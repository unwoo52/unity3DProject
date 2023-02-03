using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ChatWindow : MonoBehaviour
{
    public enum CHANNEL
    {
        Nomal, Party, Freind
    }
    Coroutine coDropDown = null;
    public Transform MyContent;
    public TMPro.TMP_InputField m_TMP_InputField;

    public TMPro.TMP_Dropdown myChanner;

    //using UnityEngine.UI;
    public UnityEngine.UI.Scrollbar m_Scrollbar;

    public void AddChat(string text)
    {
        if (text.Length <= 0) return;

        CreateTextObject(text);
        ClearInputField();
        ScrollBarDropDown();

        m_TMP_InputField.ActivateInputField();
    }

    /*codes*/

    private void ChannelMessage(StringBuilder msg, string str)
    {
        switch ((CHANNEL)myChanner.value)
        {
            case CHANNEL.Nomal:
                msg.Append("<#ffffffff>");
                msg.Append("[일반]");
                break;
            case CHANNEL.Party:
                msg.Append("<#ff0000ff>");
                msg.Append("[파티]");
                break;
            case CHANNEL.Freind:
                msg.Append("<#0000ffff>");
                msg.Append("[친구]");
                break;
        }
        msg.Append(str);
        msg.Append("</color>");
    }

    private void CreateTextObject(string text)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/ChatMessage"), MyContent) as GameObject;
        StringBuilder temp = new StringBuilder();
        ChannelMessage(temp, text);
        if (obj.TryGetComponent(out CharMessageScript sct))
        {
            sct.SetText(temp.ToString());
        }
        else
        {
            Debug.LogError("ChatMessage 오브젝트에서 CharMessageScript스크립트를 찾을 수 없습니다.");
        }
    }

    private void ClearInputField()
    {
        m_TMP_InputField.text = string.Empty;
    }

    private void ScrollBarDropDown()
    {
        //if (!m_Scrollbar.gameObject.activeSelf) return;
        if (coDropDown != null) StopCoroutine(coDropDown);
        coDropDown = StartCoroutine(ScrollBarDropDownCourutine());
    }


    IEnumerator ScrollBarDropDownCourutine()
    {
        yield return new WaitForSeconds(0.05f);
        //m_Scrollbar.value = 0.0f;
        while(m_Scrollbar.value > 0.0f)
        {
            m_Scrollbar.value -= Time.deltaTime * 4.0f;
            yield return null;
        }
    }
}
