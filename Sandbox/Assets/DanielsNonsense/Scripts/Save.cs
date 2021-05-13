using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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