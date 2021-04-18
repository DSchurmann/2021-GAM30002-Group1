using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovingPlatform obj = (MovingPlatform)target;

        if (GUILayout.Button("Set End Position"))
            obj.SetEndPosition();

    }
}
