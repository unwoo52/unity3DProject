using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;

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
        m_TMP_Text.text = InputText;

        StringBuilder temp = new StringBuilder();
        StringBuilder result = new StringBuilder();
        for(int i = 0; i < InputText.Length; ++i)
        {
            Vector2 TextSize = m_TMP_Text.GetPreferredValues(temp.ToString() + InputText[i]);
            if(TextSize.x > myRect.sizeDelta.x)
            {
                temp.Append('\n');
                result.Append(temp);
                temp.Clear();
            }
            temp.Append(InputText[i]);
        }
        result.Append(temp);

        myRect.sizeDelta = m_TMP_Text.GetPreferredValues(result.ToString());
        m_TMP_Text.text = result.ToString();
    }
    

}
