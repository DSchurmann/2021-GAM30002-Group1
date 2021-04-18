using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune 
{
    //Parameters -- Core
    public string runeName;
    public int runeIcon; //UI Icon for the Rune

    //Parameters -- Corresponding Action
    public enum RuneType { blue, red, yellow }; //Blue: Toggled. Red: Press to Act. Yellow: Contextual.
    public RuneType runeType;
    public string runeAnim; //Name of the animation to play whilst activating this rune.

    //Parameters -- Red Runes
    public enum RedRune { lift, smash }; //Add more, etc
    public RedRune runeRedType;

    //Constructor
    public Rune(string name, int iconIndex, RuneType type, string animation)
    {
        //Set Values
        runeName = name;
        runeIcon = iconIndex;
        runeType = type;
        runeAnim = animation;
    }
}
