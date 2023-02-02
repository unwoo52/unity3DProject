using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharMessageScript : MonoBehaviour
{
    public TMPro.TMP_Text m_TMP_Text;
    private RectTransform _rect = null;
    RectTransform myRect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
                //TryGetComponent(out RectTransform _rect);
            }
            return _rect;
        }
    }

    public void SetText(string InputText)
    {
        Vector2 inputTextsize = m_TMP_Text.GetPreferredValues(InputText);

        if (!IsTextLengthOverRectsize(inputTextsize))
            m_TMP_Text.text = InputText;
        else
            m_TMP_Text.text = CreateLineOverText(InputText, inputTextsize);
    }

    /*codes*/
    private string CreateLineOverText(string InputText, Vector2 inputTextsize)
    {
        int line = 1;
        RectTransform testRect = myRect;
        string temp = "";
        string result = "";

        for (int i = 0; i < InputText.Length; ++i)
        {
            Vector2 textSize = m_TMP_Text.GetPreferredValues(temp + InputText[i]);
            if (textSize.x > myRect.sizeDelta.x)
            {
                result += temp + '\n';
                temp = "";
                ++line;
            }
            temp += InputText[i];
        }
        result += temp;

        ChangeRectSize(ref testRect, line, inputTextsize);

        return result;
    }

    private void ChangeRectSize(ref RectTransform Rect, int line, Vector2 inputTextsize)
    {
        Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, (float)line * inputTextsize.y);
    }

    #region .
    private bool IsTextLengthOverRectsize(Vector2 size)
    {
        return myRect.sizeDelta.x < size.x;
    }
    #endregion
}
