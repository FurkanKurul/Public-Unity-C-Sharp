using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController instance;

    public float floatData;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/savedData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.dat", FileMode.Open);
            SaveFile data = (SaveFile)bf.Deserialize(file);
            file.Close();

            floatData = data.floatData;
        }
        else
        {
            floatData = 50.0f;
        }
    }

    public void SaveData()
    {
        if (File.Exists(Application.persistentDataPath + "/savedData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.dat",FileMode.Open);
            SaveFile data = new SaveFile();


            data.floatData = floatData;


            bf.Serialize(file, data);
            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedData.dat");
            SaveFile data = new SaveFile();


            data.floatData = floatData;


            bf.Serialize(file, data);
            file.Close();
        }
        

    }

}

[Serializable] class SaveFile
{
    public float floatData;
}
