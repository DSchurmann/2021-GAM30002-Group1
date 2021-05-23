using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour, ISave
{
    public GameObject saveObject;
    public string name;
    public int health;
    public Vector3 position;

    public SerializablePlayerSave Save()
    {
        //Debug.Log("save");
        name = "test";
        health = 10;
        saveObject = GameController.GH.CurrentPlayer().gameObject;
        position = saveObject.transform.position;
        SerializablePlayerSave save = new SerializablePlayerSave();
        save.name = name;
        save.health = health;
        save.position = position;
        return save;
    }

    public void Load(SerializablePlayerSave save)
    {
        //Debug.Log("load");
        //PlayerSave playerLoad = (PlayerSave)save;
        name = save.name;
        health = save.health;
        saveObject.transform.position = save.position;
        Debug.Log(name);
        Debug.Log(health);
        Debug.Log(position);
    }
}

[System.Serializable]
public class SerializablePlayerSave
{
    public string name;
    public int health;
    private float x;
    private float y;
    private float z;
    public Vector3 position
    {
        get
        {
            return new Vector3(x, y, z);
        }

        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }
}