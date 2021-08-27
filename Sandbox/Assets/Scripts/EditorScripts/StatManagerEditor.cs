using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(StatManager))]
public class StatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StatManager sm = (StatManager)target;
        sm.Health = sm.Health;
        sm.Damage = sm.Damage;

        //HEALTH\\
        GUILayout.BeginHorizontal(GUILayout.Height(10));
        if (sm.Health == null)
        {
            if (GUILayout.Button("Add Heatlh"))
            {
                sm.AddHealth();
            }
        }
        else
        {
            if (GUILayout.Button("Remove Health"))
            {
                sm.RemoveHealth();
            }
        }
        GUILayout.EndHorizontal();

        if (sm.Health != null)
        {
            //display health component
            GUILayout.BeginHorizontal(GUILayout.Height(10));
            EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Height(10));
            if (sm.Health != null)
            {
                sm.Health.Max = EditorGUILayout.IntField("Value:", sm.Health.Max);
            }
            GUILayout.EndHorizontal();
        }
        //DAMAGE\\
        GUILayout.BeginHorizontal(GUILayout.Height(10));
        if (sm.Damage == null)
        {
            if (GUILayout.Button("Add Damage"))
            {
                sm.AddDamage();
            }
        }
        else
        {
            if (GUILayout.Button("Remove Damage"))
            {
                sm.RemoveDamage();
            }
        }
        GUILayout.EndHorizontal();

        if (sm.Health != null)
        {
            //display health component
            GUILayout.BeginHorizontal(GUILayout.Height(10));
            EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Height(10));
            if (sm.Damage != null)
            {
                sm.Damage.Max = EditorGUILayout.IntField("Value:", sm.Damage.Max);
            }
            GUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(sm);
            EditorSceneManager.MarkSceneDirty(sm.gameObject.scene);
        }
    }
}
#endif