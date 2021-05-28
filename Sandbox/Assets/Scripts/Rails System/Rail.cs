using System;
using UnityEngine;

[ExecuteInEditMode]
public class Rail : MonoBehaviour
{
    [Header("Node Properties")]
    [Range(0.1f,1f)]
    public float NodeSize = 0.25f;

    private Transform[] nodes;

    private void Awake()
    {
        nodes = Array.FindAll(GetComponentsInChildren<Transform>(), NotThis);
    }

    public Vector3 Rotate(int seg, float d)
    {
        Vector3 result = Vector3.zero;
        if (d < 0)
        {
            result = nodes[seg].position;
        }
        else if (d > 0)
        {
            result = nodes[seg + 1].position;
        }
        return result;
    }

    public Vector3 Move(int seg, float speed)
    {
        Vector3 v1 = nodes[seg].position;
        Vector3 v2 = nodes[seg + 1].position;

        return Vector3.Lerp(v1, v2, speed);
    }

    public Vector3 CatmullMove(int seg, float speed)
    {
        Vector3 p1, p2, p3, p4;

        if (seg == 0)
        {
            p1 = nodes[seg].position;
            p2 = p1;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 1].position;
        }
        else if (seg == nodes.Length - 2)
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = p3;
        }
        else
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }

        float t2 = speed * speed;
        float t3 = t2 * speed;

        float x = 0.5f * ((2.0f * p2.x) //this is catmull-rom equation see => https://www.mvps.org/directx/articles/catmull/ for details
            + (-p1.x + p3.x) * speed 
            + (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x) * t2
            + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) * t3);

        /*
         float y = 0.5f * ((2.0f * p2.y)
            + (-p1.y + p3.y) * speed 
            + (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y)* t2
            + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y) * t3);
        
         */
        float z = 0.5f * ((2.0f * p2.z)
            + (-p1.z + p3.z) * speed 
            + (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z) * t2 
            + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z) * t3);

        return new Vector3(x, 0, z);
    }

    //Onscreen display for editor
    private void OnDrawGizmos()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            UnityEditor.Handles.DrawWireCube(nodes[i].position, Vector3.one* NodeSize);
            if (i < nodes.Length - 1)
            {
                UnityEditor.Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, NodeSize*4);
            }
        }

        for(int i = 0; i < nodes.Length - 1; i++)
        {
            float temp = 0;
            for(int j = 0; j < 20; j++)
            {
                float f = 0;
                if(j > 0)
                {
                    f = (float)j / 20;
                }
                UnityEditor.Handles.DrawLine(CatmullMove(i, temp), CatmullMove(i, f + 0.05f));
                temp = f;
            }
        }
    }

    private bool NotThis(Transform t)
    {
        return t != this.transform;
    }

    public Vector3 GetNodePos(int i)
    {
        return nodes[i].position;
    }

    public int NodeLength
    {
        get { return nodes.Length; }
    }

    public string getName(int i)
    {
        return nodes[i].name;
    }

}
