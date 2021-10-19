using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private const float nodeConnectionDistance = 2.0f; 

    // give each node a reference to their rail
    public Rail rail { get; set; }

    public List<Node> linkToConnectingNodes = new List<Node>();

    public int GetNodeIndex()
    {
        return rail.Nodes.IndexOf(this);
    }

    public void UpdateConnectingNodes()
    {
        linkToConnectingNodes.Clear();

        // get the index of the node 
        int index = rail.GetComponent<DrawRailPath>().Nodes.IndexOf(gameObject);
        
        // previous node 
        if (index > 0)
        {
            linkToConnectingNodes.Add(rail.GetComponent<DrawRailPath>().Nodes[index - 1].GetComponent<Node>());
        }

        // next node
        if (index < rail.GetComponent<DrawRailPath>().Nodes.Count - 1)
        {
            linkToConnectingNodes.Add(rail.GetComponent<DrawRailPath>().Nodes[index + 1].GetComponent<Node>());
        }

        // branching nodes
        foreach (Node n in FindObjectsOfType<Node>())
        {
            // test if node is close to self
            if (Vector3.Distance(new Vector3(n.transform.position.x, 0f, n.transform.position.z) , new Vector3(transform.position.x, 0f, transform.position.z)) < nodeConnectionDistance)
            {
                // test node is not self
                if (n != this)
                {
                    linkToConnectingNodes.Add(n);
                }
            }
        }
    }
}
