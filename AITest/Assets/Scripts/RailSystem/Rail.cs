using System;
using UnityEngine;

[ExecuteInEditMode]
public class Rail : MonoBehaviour
{
    private Transform[] nodes;

    void Start()
    {
        nodes = Array.FindAll(GetComponentsInChildren<Transform>(), NotThis);
    }

    public Vector3 Move(int seg, float speed)
    {
        Vector3 v1 = nodes[seg].position;
        Vector3 v2 = nodes[seg + 1].position;

        return Vector3.Lerp(v1, v2, speed);
    }

    //Onscreen display for editor
    private void OnDrawGizmos()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            UnityEditor.Handles.DrawWireCube(nodes[i].position, Vector3.one);
            if (i < nodes.Length - 1)
            {
                UnityEditor.Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 4f);
            }
        }
    }

    private bool NotThis(Transform t)
    {
        return t != this.transform;
    }

}
