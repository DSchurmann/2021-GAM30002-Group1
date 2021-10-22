using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(MovingPlatform))]
public class MovingObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovingPlatform obj = (MovingPlatform)target;


        if (GUILayout.Button("Set Start Position"))
            obj.SetStartPosition();

        if (GUILayout.Button("Set Between Position"))
            obj.setPlatformPosition();

        if (GUILayout.Button("Set End Position"))
            obj.SetEndPosition();

        if (GUILayout.Button("Reset Position"))
            obj.resetPosition();
    }
}
#endif
