using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatManager))]
public class StatManagerEditor : Editor
{
    StatType statType;
    public override void OnInspectorGUI()
    {
        StatManager sm = (StatManager)target;

        foreach(Stat s in sm.GetStats)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Stat", EditorStyles.boldLabel);
            if (GUILayout.Button("Remove Stat"))
            {
                sm.GetStats.Remove(s);
            }
            GUILayout.EndHorizontal();

            s.Type = (StatType)EditorGUILayout.EnumPopup("Type:", s.Type);
            s.Max = EditorGUILayout.FloatField("Value:", s.Max);
            GUILayout.Space(10);
        }

        if(GUILayout.Button("Add Stat"))
        {
            sm.GetStats.Add(new Stat());
        }
    }
}
