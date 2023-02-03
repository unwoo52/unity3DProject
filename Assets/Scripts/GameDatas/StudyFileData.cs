using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StudyPlayerData
{
    public string Nick;
    public int Gold;
    public int Exp;
}

public class StudyFileData : MonoBehaviour
{
    public TMPro.TMP_Text Nick = null;
    public TMPro.TMP_Text Gold = null;
    public TMPro.TMP_Text Exp = null;
    bool TestBool = false;
    void Start()
    {
        // StudyFileManager.instance.SaveText(Application.dataPath + "\\Test.txt", "이건 테스트 입니다.");
        //myLabel.text = StudyFileManager.instance.LoadText(Application.dataPath + "\\Test.txt");

        if(TestBool)
        {
            StudyPlayerData data = new StudyPlayerData();
            data.Nick = "Nickname";
            data.Gold = 1000;
            data.Exp = 10;

            StudyFileManager.instance.SaveBinary<StudyPlayerData>(Application.dataPath + @"\TestPlayerData.playerdatafile", data);
        }

        StudyPlayerData studyPlayerData = StudyFileManager.instance.LoadBinary<StudyPlayerData>(Application.dataPath + @"\TestPlayerData.playerdatafile");

        Nick.text = studyPlayerData.Nick;
        Gold.text = studyPlayerData.Gold.ToString();
        Exp.text = studyPlayerData.Exp.ToString();




    }

    // Update is called once per frame
    void Update()
    {

    }
}


