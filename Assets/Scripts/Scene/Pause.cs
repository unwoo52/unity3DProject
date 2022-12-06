using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Player;
using System.IO;
using System;

public class Pause : MonoBehaviour
{
    public Canvas myCanvas = null;
    public List<GameObject> myPause = new List<GameObject>();
    bool IsActive = false;
    public Image mySaveAlarm = null;
    public Image myKeySaveAlarm = null;

    public enum MenuState
    { 
        Main, Save, Settings, KeySettings
    }

    public MenuState myMenu = MenuState.Main;

    void ChangeMenu(MenuState state)
    {
        if (myMenu == state) return;
        myMenu = state;
        switch (myMenu)
        {
            case MenuState.Main:
                foreach(GameObject n in myPause)
                {
                    n.SetActive(false);
                }
                myPause[(int)MenuState.Main].SetActive(true);
                break;
            case MenuState.Save:
                foreach (GameObject n in myPause)
                {
                    n.SetActive(false);
                }
                myPause[(int)MenuState.Save].SetActive(true);
                break;
            case MenuState.Settings:
                foreach (GameObject n in myPause)
                {
                    n.SetActive(false);
                }
                myPause[(int)MenuState.Settings].SetActive(true);
                break;
            case MenuState.KeySettings:
                foreach (GameObject n in myPause)
                {
                    n.SetActive(false);
                }
                myPause[(int)MenuState.KeySettings].SetActive(true);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerScript.instance.State.isCursorActive = !PlayerScript.instance.State.isCursorActive;
            IsActive = !IsActive;
            myCanvas.gameObject.SetActive(IsActive);
        }
    }

    public void ClickMainPlay()
    {
        PlayerScript.instance.State.isCursorActive = !PlayerScript.instance.State.isCursorActive;
        IsActive = !IsActive;
        myCanvas.gameObject.SetActive(IsActive);
    }

    public void ClickMainGotoMain()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ClickMainQuit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }

    public void ClickMainSave()
    {
        ChangeMenu(MenuState.Save);
    }

    public void ClickSaveBack()
    {
        ChangeMenu(MenuState.Main);
    }

    public void ClickSaveFile1()
    {
        PlayerData data = new PlayerData();
        data.curHP = PlayerScript.instance.myInfo.CurHP;
        data.curPos = PlayerScript.instance.transform.position;

        FileManager.Inst.SaveFile(Application.dataPath + @"\SaveFile1.data", data);

        StartCoroutine(Alarm(mySaveAlarm.gameObject));
    }

    public void ClickSaveFile2()
    {
        PlayerData data = new PlayerData();
        data.curHP = PlayerScript.instance.myInfo.CurHP;
        data.curPos = PlayerScript.instance.transform.position;

        FileManager.Inst.SaveFile(Application.dataPath + @"\SaveFile2.data", data);

        StartCoroutine(Alarm(mySaveAlarm.gameObject));
    }

    IEnumerator Alarm(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        go.SetActive(false);
    }

    public void ClickSettingsKeySettings()
    {
        ChangeMenu(MenuState.KeySettings);
    }

    public void ClickKeySettingsBack()
    {
        ChangeMenu(MenuState.Settings);
    }

    public void ClickMainSettings()
    {
        ChangeMenu(MenuState.Settings);
    }

    public void ClickKeySettingsSave()
    {
        KeyManager.Inst.KeySave(Application.dataPath + @"SettingData.data");
        StartCoroutine(Alarm(myKeySaveAlarm.gameObject));
    }
}
