using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class GameManager : MonoBehaviour
{
    private string SAVE_FOLDER;
    public static GameManager saveLoadManager;

    private void Awake()
    {
        if (saveLoadManager == null)
        {
            saveLoadManager = this;
        }
        else if (saveLoadManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SAVE_FOLDER = Application.dataPath + "/save.sav";
    }

    public void Save()
    {
        Save save = new Save();
        

        ISave[] saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISave>().ToArray();
        foreach (ISave savable in saveables)
        {
            // put all the json files into a json file
            save.saves.Add(savable.Save()); 
        }
        Debug.Log(SAVE_FOLDER);
        //Debug.Log(JsonUtility.ToJson(save));
        //File.WriteAllText(SAVE_FOLDER, JsonUtility.ToJson(save));


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(SAVE_FOLDER);
        bf.Serialize(file, save);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(SAVE_FOLDER))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SAVE_FOLDER, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            ISave[] saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISave>().ToArray();
            foreach (ISave savable in saveables)
            {                

                foreach (SerializablePlayerSave savedObject in save.saves)
                {

                    // TODO some sort of search to ensure the correct object is saved
                    savable.Load(savedObject);
                }
            }

            
        }
    }
}

[System.Serializable]
public class Save
{
    public List<SerializablePlayerSave> saves;
    // some game settings 

    public Save()
    {
        saves = new List<SerializablePlayerSave>();
    }
}
