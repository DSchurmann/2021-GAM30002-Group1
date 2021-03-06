using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rail : MonoBehaviour
{
    [SerializeField] private int priority = 0;
    public bool enable = true;
    public bool swapControls = false;

    private List<Transform> nodes;
    [SerializeField] private RailType type;

    private void Awake()
    {
        nodes = new List<Transform>();

        if(GetComponent<DrawRailPath>() != null)
        {
            foreach (GameObject g in GetComponent<DrawRailPath>().Nodes)
            {
                nodes.Add(g.transform);
            }
        }
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

    public float GetSegmentLength(int seg)
    {
        const int resolution = 7;
        float dist = 0.0f;
        float incrementAmount = 1.0f / (float)resolution;
        for (int i = 0; i < resolution; i++)
        {
            dist += Vector3.Distance(CatmullMove(seg, incrementAmount * i), CatmullMove(seg, incrementAmount * (i + 1)));
        }

        return dist;
    }

    public int GetSegmentOfClosestPoint(Vector3 pt, float accuracy = 0.5f)
    {
        int nodeOfClosestPoint = 0;
        float bestDistance = float.PositiveInfinity;

        for (int seg = 0; seg < nodes.Count - 1; seg++)
        {
            float aproxSegmentDist = GetSegmentLength(seg);

            int ndivs = Mathf.Max((int)(aproxSegmentDist / accuracy), 5); // the min is to account for the linear distance if the distance to to close the player.

            for (int i = 0; i <= ndivs; i++)
            {
                float t = (float)i / (float)ndivs;
                Vector3 checkedPoint = CatmullMove(seg, t);
                float dist = Vector3.Distance(checkedPoint, pt);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    nodeOfClosestPoint = seg;
                }
            }
        }

        return nodeOfClosestPoint;
    }

    public bool IsRailWithinRange(Vector3 pt, float range, bool includeEnds = true, float accuracy = 0.5f)
    {
        if (!enable)
            return false;

        Vector3 closestPoint = ClosestPointOnCatmullRom(pt, accuracy);

        if (!includeEnds)
        {
            /*Debug.Log("endPoints");
            Debug.Log(closestPoint);
            Debug.Log(nodes[0].position);
            Debug.Log(nodes[NodeLength - 1].position);*/

            // check if closest point is either of the ends
            Vector3 negateY = new Vector3(1, 0, 1);
            if (closestPoint == Vector3.Scale(nodes[0].position, negateY) || closestPoint == Vector3.Scale(nodes[NodeLength - 1].position, negateY))
            {
                //Debug.Log("end");
                return false;
            }
        }

        float dist = Vector3.Distance(closestPoint, pt);
        return dist <= range;
    }

    public Vector3 ClosestPointOnCatmullRom(Vector3 pt, float accuracy = 0.5f)
    {
        Vector3 closestPoint = new Vector3();
        float bestDistance = float.PositiveInfinity;

        for (int seg = 0; seg < nodes.Count - 1; seg++)
        {
            float aproxSegmentDist = GetSegmentLength(seg);

            int ndivs = Mathf.Max((int)(aproxSegmentDist / accuracy), 5); // the min is to account for the linear distance if the distance to to close the player.

            for (int i = 0; i <= ndivs; i++)
            {
                float t = (float)i / (float)ndivs;
                Vector3 checkedPoint = CatmullMove(seg, t);
                float dist = Vector3.Distance(checkedPoint, pt);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closestPoint = checkedPoint;
                }
            }
        }

        return closestPoint;
    }

    public Vector3 ClosestPointOnCatmullRom(Vector3 pt, int seg, float accuracy = 0.5f)
    {
        // set the height to zero so we only look at the player
        pt.y = 0;

        //float aproxSegmentDist = Vector3.Distance(nodes[seg].position, nodes[seg + 1].position);
        float aproxSegmentDist = GetSegmentLength(seg);

        int ndivs = Mathf.Max((int)(aproxSegmentDist / accuracy), 5); // the min is to account for the linear distance if the distance to to close the player.

        Vector3 result = new Vector3();
        float bestDistance = float.PositiveInfinity;
        for (int i = 0; i <= ndivs; i++)
        {
            float t = (float)i / (float)ndivs;
            Vector3 checkedPoint = CatmullMove(seg, t);
            float dist = Vector3.Distance(checkedPoint, pt);
            if (dist < bestDistance)
            {
                bestDistance = dist;
                result = checkedPoint;
            }
        }
        return result;
    }

    public float ClosestPointOnCatmullRomAsPercent(Vector3 pt, int seg, float accuracy = 0.5f)
    {
        // set the height to zero so we only look at the player
        pt.y = 0;

        //float aproxSegmentDist = Vector3.Distance(nodes[seg].position, nodes[seg + 1].position);
        float aproxSegmentDist = GetSegmentLength(seg);

        int ndivs = Mathf.Max((int)(aproxSegmentDist / accuracy), 5); // the min is to account for the linear distance if the distance to to close the player.

        float result = 0;
        float bestDistance = float.PositiveInfinity;
        for (int i = 0; i <= ndivs; i++)
        {
            float t = (float)i / (float)ndivs;
            Vector3 checkedPoint = CatmullMove(seg, t);
            float dist = Vector3.Distance(checkedPoint, pt);
            if (dist < bestDistance)
            {
                bestDistance = dist;
                result = t;
            }
        }
        return result;
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
        else if (seg == nodes.Count - 2)
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

    private bool NotThis(Transform t)
    {
        return t != this.transform;
    }

    public int NodeLength
    {
        get { return nodes.Count; }
    }

    public int Priority
    {
        get { return priority; }
        set { priority = value; }
    }

    public RailType Type
    { 
        get { return type; }
        set { type = value; }
    }
}