using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Initialize()
    {
        string path = Application.persistentDataPath;

        if(!File.Exists(path + "/Save1.ava"))
        {
            CreateSaveState(path + "/Save1.ava");
        }
        if (!File.Exists(path + "/Save2.ava"))
        {
            CreateSaveState(path + "/Save2.ava");
        }
        if (!File.Exists(path + "/Save3.ava"))
        {
            CreateSaveState(path + "/Save3.ava");
        }
    }

    private static void CreateSaveState(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void Save (Player player)
    {
        string path = Application.persistentDataPath + "/Save.ava";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        SaveData data = new SaveData(player);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/Save1.ava";

        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = bf.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("No Savefile");
            return null;
        }
    }

    public static SaveData[] LoadAll()
    {
        SaveData[] data = new SaveData[3];
        string path = Application.persistentDataPath;
        BinaryFormatter bf = new BinaryFormatter();


        FileStream stream = new FileStream(path + "/Save1.ava", FileMode.Open);
        data[0] = bf.Deserialize(stream) as SaveData;
        
        stream = new FileStream(path + "/Save2.ava", FileMode.Open);
        data[1] = bf.Deserialize(stream) as SaveData;
        
        stream = new FileStream(path + "/Save3.ava", FileMode.Open);
        data[2] = bf.Deserialize(stream) as SaveData;
        
        stream.Close();
        return data;
    }
}
