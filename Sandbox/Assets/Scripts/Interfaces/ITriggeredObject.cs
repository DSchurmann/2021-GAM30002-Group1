using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggeredObject
{
    void Trigger(bool value, int item = 0);
}
