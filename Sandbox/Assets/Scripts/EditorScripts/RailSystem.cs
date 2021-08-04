using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
[InitializeOnLoad]
public class RailSystem : EditorWindow
{
    [SerializeField] private List<RailContainer> rails = new List<RailContainer>();
    private Vector2 scrollpos;
    private float nodeSize = 1f;
    private string sceneName;

    [MenuItem("Tools/Level Tools/Rail System")]
    public static void ShowWindow()
    {
        GetWindow<RailSystem>("Rail System");
    }

    private void Awake()
    {
        GetRailDataFromObjects();
    }

    private void OnEnable()
    {
        GetRailDataFromObjects();
    }

    private void OnHierarchyChange()
    {
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            Debug.Log("scene should have changed");
            GetRailDataFromObjects();
        }
    }

    private void OnGUI()
    {
        //scroll bar
        scrollpos = GUILayout.BeginScrollView(scrollpos, false, false);
        GUILayout.BeginHorizontal();
            //add rail button to add a new rail with a 4 nodes at the same position (so catmull works)
            if (GUILayout.Button("Add New Rail"))
            {
            //create game object
            GameObject g = new GameObject("Rail" + rails.Count);
            //change tag
            g.tag = "Rail";
            //add required components
            g.AddComponent<Rail>();
            g.AddComponent<DrawRailPath>();
            Color c = new Color((float)UnityEngine.Random.Range(0, 255), (float)UnityEngine.Random.Range(0, 255), (float)UnityEngine.Random.Range(0, 255));
            g.GetComponent<DrawRailPath>().Colour = c;
            //setup rail container
            RailContainer r = new RailContainer(g, c);
            //add nodes
            for (int i = 0; i < 4; i++)
            {
                r.AddNode(nodeSize);
            }
            //add container to list, increment count and set the rail to the active object in the scene
            rails.Add(r);
            Selection.activeObject = r.GetRail;
            }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        if (rails.Count > 0)
        {
            //check if the node size has been changed before changing it for all nodes, prevents it being changed 10 times a second
            EditorGUI.BeginChangeCheck();
            nodeSize = EditorGUILayout.Slider("Node Size", nodeSize, 0, 10);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (RailContainer rc in rails)
                {
                    rc.ChangeNodeSize(nodeSize);
                }
            }
            GUILayout.Space(30);

            foreach (RailContainer g in rails)
            {
                //remove any rails that have been deleted via the hierachy
                if (g == null || !GameObject.FindGameObjectsWithTag("Rail").Contains(g.GetRail))
                {
                    rails.Remove(g);
                    break;
                }

                //horizontal for rail details, name and delete rail button
                GUILayout.BeginHorizontal();
                GUILayout.Label("Rail Name:");
                g.GetRail.name = EditorGUILayout.TextField(g.GetRail.name);
                if(GUILayout.Button("Remove Rail"))
                {
                    DestroyImmediate(g.GetRail);
                    rails.Remove(g);
                    break;
                }
                GUILayout.EndHorizontal();

                //horizontal for rail object reference, for selection and colour of the scene GUI
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(g.GetRail.name, GUILayout.Width(150)))
                {
                    Selection.activeObject = g.GetRail;
                }
                GUILayout.Space(100);
                g.Colour = EditorGUILayout.ColorField(g.Colour, GUILayout.Width(50));
                GUILayout.EndHorizontal();

                GUILayout.Label("Nodes");
                foreach(GameObject n in g.GetNodes)
                {
                    //remove any nodes that have been deleted via the hierachy
                    if(n == null)
                    {
                        g.GetNodes.Remove(n);
                        break;
                    }
                    GUILayout.BeginHorizontal();
                    //if button is pressed, corresponding node is selected in the scene view
                    if(GUILayout.Button(n.name, GUILayout.Width(100)))
                    {
                        Selection.activeObject = n;
                    }
                    GUILayout.Space(100);
                    //delete node button
                    if(GUILayout.Button("Delete Node", GUILayout.Width(100)))
                    {
                        DestroyImmediate(n);
                        g.GetNodes.Remove(n);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }

                //button for adding a node
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Node"))
                {
                    g.AddNode(nodeSize);
                    Selection.activeObject = g.GetNodes[g.GetNodes.Count - 1];
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(40);
            }
        }
        GUILayout.EndScrollView();
    }

    private void GetRailDataFromObjects()
    {
        //find all rails with tag - for when openning the window after having nodes
        GameObject[] g = GameObject.FindGameObjectsWithTag("Rail");
        if (g.Length != 0)
        {
            //empty rails list just in case
            rails.Clear();

            foreach (GameObject r in g)
            {
                //create new rail container with gameobject and colour from existing object
                RailContainer rc = new RailContainer(r, r.GetComponent<DrawRailPath>().Colour);
                List<GameObject> nodes = new List<GameObject>();
                //get all nodes from said object
                Transform[] child = r.GetComponentsInChildren<Transform>();
                //if there are nodes, add game object to list
                if (child != null)
                {
                    nodeSize = child[1].GetComponent<DrawNode>().NodeSize;
                    foreach (Transform t in child)
                    {
                        if (t != r.transform)
                        {
                            nodes.Add(t.gameObject);
                        }
                    }
                }
                //add nodes to rail containter
                foreach (GameObject n in nodes)
                {
                    rc.AddNode(n);
                }
                //add conatiner to list
                rails.Add(rc);
                //increment count
            }
        }
        sceneName = SceneManager.GetActiveScene().name;
    }

    [Serializable]
    private class RailContainer
    {
        private readonly GameObject obj;
        private Color colour;

        public RailContainer(GameObject r, Color c)
        {
            obj = r;
            colour = c;
            obj.GetComponent<DrawRailPath>().Nodes = new List<GameObject>();
        }

        public void AddNode(float size)
        {
            //create new object and set parent to rail object
            GameObject n = new GameObject("Node" + obj.GetComponent<DrawRailPath>().Nodes.Count);
            n.transform.parent = obj.transform;
            //if this isn't the first node, set the transform to the last node in the list
            if(obj.GetComponent<DrawRailPath>().Nodes.Count > 0)
            {
                n.transform.position = obj.GetComponent<DrawRailPath>().Nodes[obj.GetComponent<DrawRailPath>().Nodes.Count - 1].transform.position;
            }

            //add draw node script and set the colour to the same as the parent
            n.AddComponent<DrawNode>();
            n.GetComponent<DrawNode>().SetColour = colour;
            n.GetComponent<DrawNode>().NodeSize = size;

            //add node to list and increment childCount
            obj.GetComponent<DrawRailPath>().Nodes.Add(n);
            SceneView.RepaintAll();
        }

        public void AddNode(GameObject n)
        {
            //set parent, add node
            n.transform.parent = obj.transform;
            obj.GetComponent<DrawRailPath>().Nodes.Add(n);
            SceneView.RepaintAll();
        }

        public void ChangeNodeSize(float s)
        {
            //change the node size for each node
            foreach(GameObject n in obj.GetComponent<DrawRailPath>().Nodes)
            {
                n.GetComponent<DrawNode>().NodeSize = s;
            }
            SceneView.RepaintAll();
        }

        public GameObject GetRail
        {
            get { return obj; }
        }
        public Color Colour
        {
            get { return colour; }
            set {
                //change colour for the container, rail path and nodes
                colour = value; 
                obj.GetComponent<DrawRailPath>().Colour = value;
                foreach(GameObject n in obj.GetComponent<DrawRailPath>().Nodes)
                {
                    n.GetComponent<DrawNode>().SetColour = value;
                }
            }
        }

        public List<GameObject> GetNodes
        {
            get { return obj.GetComponent<DrawRailPath>().Nodes; }
        }
    }
}
#endif