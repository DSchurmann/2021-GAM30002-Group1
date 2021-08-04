using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DrawRailPath : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Color colour;
    [SerializeField] private Rail rail;
    [SerializeField] private List<GameObject> nodes = new List<GameObject>();

    private void Awake()
    {
        runInEditMode = true;
        Transform[] child = GetComponentsInChildren<Transform>();
        //if there are nodes, add game object to list
        if (child != null)
        {
            foreach (Transform t in child)
            {
                if (t != transform)
                {
                    nodes.Add(t.gameObject);
                }
            }
        }
    }

    private void Start()
    {
        Transform[] child = GetComponentsInChildren<Transform>();
        //if there are nodes, add game object to list
        if (child != null)
        {
            foreach (Transform t in child)
            {
                if (t != transform)
                {
                    nodes.Add(t.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = colour;

        //draw the curve that the characters will traverse
        for (int i = 0; i < rail.NodeLength - 1; i++)
        {
            float temp = 0;
            for (int j = 0; j < 20; j++)
            {
                float f = 0;
                if (j > 0)
                {
                    f = (float)j / 20;
                }
                Gizmos.DrawLine(CatmullMove(i, temp), CatmullMove(i, f + 0.05f));
                temp = f;
            }
        }
    }

    public Vector3 CatmullMove(int seg, float speed)
    {
        Vector3 p1, p2, p3, p4;

        if (seg == 0)
        {
            p1 = nodes[seg].transform.position;
            p2 = p1;
            p3 = nodes[seg + 1].transform.position;
            p4 = nodes[seg + 1].transform.position;
        }
        else if (seg == nodes.Count - 2)
        {
            p1 = nodes[seg - 1].transform.position;
            p2 = nodes[seg].transform.position;
            p3 = nodes[seg + 1].transform.position;
            p4 = p3;
        }
        else
        {
            p1 = nodes[seg - 1].transform.position;
            p2 = nodes[seg].transform.position;
            p3 = nodes[seg + 1].transform.position;
            p4 = nodes[seg + 2].transform.position;
        }

        float t2 = speed * speed;
        float t3 = t2 * speed;

        float x = 0.5f * ((2.0f * p2.x) //this is catmull-rom equation see => https://www.mvps.org/directx/articles/catmull/ for details
            + (-p1.x + p3.x) * speed
            + (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x) * t2
            + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) * t3);

        float z = 0.5f * ((2.0f * p2.z)
            + (-p1.z + p3.z) * speed
            + (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z) * t2
            + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z) * t3);

        return new Vector3(x, 0, z);
    }

    public Rail SetRail
    {
        set { rail = value; }
    }

    public Color Colour
    {
        get { return colour; }
        set { colour = value; }
    }

    public List<GameObject> Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }
#endif
}