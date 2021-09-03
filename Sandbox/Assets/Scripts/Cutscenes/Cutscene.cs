using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene: MonoBehaviour
{
    //Parameters -- Core
    public string cutsceneName;
    public List<CutEvent> cutsceneEvents = new List<CutEvent>();

    //Constructor
    public Cutscene(string name)
    {
        //Set Name
        cutsceneName = name;

        //Events: Initialize
        cutsceneEvents = new List<CutEvent>();
    }
}
