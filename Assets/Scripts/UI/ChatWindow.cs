﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWindow : MonoBehaviour
{
    public Transform MyContent;
    public TMPro.TMP_InputField m_TMP_InputField;

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

    private void CreateTextObject(string text)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/ChatMessage"), MyContent) as GameObject;
        if (obj.TryGetComponent(out CharMessageScript sct))
        {
            sct.SetText(text);
        }
    }

    private void ClearInputField()
    {
        m_TMP_InputField.text = string.Empty;
    }

    private void ScrollBarDropDown()
    {
        //if (!m_Scrollbar.gameObject.activeSelf) return;
        StartCoroutine(ScrollBarDropDownCourutine());
    }


    IEnumerator ScrollBarDropDownCourutine()
    {
        yield return new WaitForSeconds(0.05f);
        //m_Scrollbar.value = 0.0f;
        while(m_Scrollbar.value > 0.0f)
        {
            m_Scrollbar.value -= Time.deltaTime * 2.0f;
            yield return null;
        }
    }
}
