using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class StudyFileManager : MonoBehaviour
{
    public static StudyFileManager instance = null;
    BinaryFormatter binaryFormatter = null;
    private void Awake()
    {
        instance = this;
        binaryFormatter= new BinaryFormatter();
    }

    public void SaveText(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }

    public string LoadText(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public void SaveBinary<T>(string filePath, T data)
    {
        using (FileStream fileStream = File.Create(filePath))
        {
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
        
    }

    public T LoadBinary<T>(string filePath)
    {
        T data = default;
        if (File.Exists(filePath))
        {
            using(FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                data = (T)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
        }
        else
        {
            Debug.LogError(filePath + "파일이 존재하지 않습니다.");
        }
        return data;
    }
}
