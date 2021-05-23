using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISave
{
    SerializablePlayerSave Save();
    void Load(SerializablePlayerSave save);
}
