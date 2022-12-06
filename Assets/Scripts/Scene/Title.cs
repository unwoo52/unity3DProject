using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Title : MonoBehaviour
{
    public List<GameObject> myMain = new List<GameObject>();
    public Image myAlarmError = null;
    public Image myAlarmSave = null;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum MenuState
    {
        Main, Start, Delete, Settings, KeySettings
    }

    public MenuState myMenu = MenuState.Main;

    void ChangeMenu(MenuState state)
    {
        if (myMenu == state) return;
        myMenu = state;
        switch (myMenu)
        {
            case MenuState.Main:
                foreach (GameObject n in myMain)
                {
                    n.SetActive(false);
                }
                myMain[(int)MenuState.Main].SetActive(true);
                break;
            case MenuState.Start:
                foreach (GameObject n in myMain)
                {
                    n.SetActive(false);
                }
                myMain[(int)MenuState.Start].SetActive(true);
                break;
            case MenuState.Delete:
                foreach (GameObject n in myMain)
                {
                    n.SetActive(false);
                }
                myMain[(int)MenuState.Delete].SetActive(true);
                break;
            case MenuState.Settings:
                foreach (GameObject n in myMain)
                {
                    n.SetActive(false);
                }
                myMain[(int)MenuState.Settings].SetActive(true);
                break;
            case MenuState.KeySettings:
                foreach (GameObject n in myMain)
                {
                    n.SetActive(false);
                }
                myMain[(int)MenuState.KeySettings].SetActive(true);
                break;
        }
    }

    public void ClickMainStart()
    {
        ChangeMenu(MenuState.Start);
    }

    public void ClickMainQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }

    public void ClickMainSettings()
    {
        ChangeMenu(MenuState.Settings);
    }

    public void ClickMainDelete()
    {
        ChangeMenu(MenuState.Delete);
    }

    public void ClickStartNewGame()
    {
        LoadManager.Inst.ChangeScene(2);
    }

    public void ClickStartSave1()
    {
        FileData.Inst.curData = FileManager.Inst.LoadFile<PlayerData>(Application.dataPath + @"\SaveFile1.data");

        if (FileManager.Inst.IsExist)
        {
            LoadManager.Inst.ChangeScene(2);
        }
        else
        {
            StartCoroutine(Alarm(myAlarmError.gameObject));
        }
    }

    public void ClickStartSave2()
    {
        FileData.Inst.curData = FileManager.Inst.LoadFile<PlayerData>(Application.dataPath + @"\SaveFile2.data");

        if (FileManager.Inst.IsExist)
        {
            LoadManager.Inst.ChangeScene(2);
        }
        else
        {
            StartCoroutine(Alarm(myAlarmError.gameObject));
        }
    }

    IEnumerator Alarm(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        go.gameObject.SetActive(false);
    }

    public void ClickStartBack()
    {
        ChangeMenu(MenuState.Main);
    }

    public void ClickDeleteSF1()
    {
        File.Delete(Application.dataPath + @"\SaveFile1.data");
    }

    public void ClickDeleteSF2()
    {
        File.Delete(Application.dataPath + @"\SaveFile2.data");
    }

    public void ClickSettingsKeySettings()
    {
        ChangeMenu(MenuState.KeySettings);
    }

    public void ClickKeySettingsBack()
    {
        ChangeMenu(MenuState.Settings);
    }

    public void ClickKeySettingsSave()
    {
        KeyManager.Inst.KeySave(Application.dataPath + @"SettingData.data");
        StartCoroutine(Alarm(myAlarmSave.gameObject));
    }
}
