using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySettingManager : MonoBehaviour
{
    public TMP_Text[] txt;

    int key = -1;

    // Start is called before the first frame update
    private void Awake()
    {
        for(int i = 0; i < txt.Length; i++)
        {
            txt[i].text = KeyManager.Inst.KeyPairs[(KeyAction)i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < txt.Length; i++)
        {
            txt[i].text = KeyManager.Inst.KeyPairs[(KeyAction)i].ToString();
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if (keyEvent.isKey)
        {
            KeyManager.Inst.KeyPairs[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
        }
    }

    public void ChangeKey(int num)
    {
        key = num;
    }
}
