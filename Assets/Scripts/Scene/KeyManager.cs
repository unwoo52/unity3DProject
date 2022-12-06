using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public enum KeyAction
{
    Forward, Backward, Left, Right, Run, Jump, SwitchCamera, ShowCursor, LeftClick, RightClick, RollingBody, Esc, KeyCount
}

[Serializable]
public class KeySetting
{
    public List<KeyCode> keys = new List<KeyCode>();
}

public class KeyManager : MonoBehaviour
{
    public KeySetting myKeys = new KeySetting();
    public Dictionary<KeyAction, KeyCode> KeyPairs = new Dictionary<KeyAction, KeyCode>();
    public static KeyManager Inst = null;

    KeyCode[] defaultKeys = new KeyCode[] 
    { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.LeftShift, 
        KeyCode.F, KeyCode.V, KeyCode.C, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Q, KeyCode.Escape };

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }

        string path = Application.dataPath + @"SettingData.data";

        if (!File.Exists(path))
        {
            for (int i = 0; i < (int)KeyAction.KeyCount; i++)
            {
                myKeys.keys.Add(defaultKeys[i]);
            }

            for (int i = 0; i < (int)KeyAction.KeyCount; i++)
            {
                KeyPairs.Add((KeyAction)i ,defaultKeys[i]);
            }

            string data = JsonUtility.ToJson(myKeys);

            File.Create(path).Close();
            File.WriteAllText(path, data);
        }
        else
        {
            KeyLoad(path);
        }
    }

    public void KeySave(string path)
    {
        myKeys.keys.Clear();

        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            myKeys.keys.Add(KeyPairs[(KeyAction)i]);
        }

        string data = JsonUtility.ToJson(myKeys);

        File.WriteAllText(path, data);
    }

    public void KeyLoad(string path)
    {
        string data = File.ReadAllText(path);

        myKeys = JsonUtility.FromJson<KeySetting>(data);

        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            KeyPairs.Add((KeyAction)i, myKeys.keys[i]);
        }
    }
}
