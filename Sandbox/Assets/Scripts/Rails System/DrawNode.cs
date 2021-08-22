using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class DrawNode: MonoBehaviour
{
    [SerializeField] private Color colour;
    [SerializeField] private float nodeSize = 1f;
    
    private void Start()
    {
        SceneView.RepaintAll();
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = colour;
        Gizmos.DrawCube(transform.position, Vector3.one * nodeSize);

    }

    public Color SetColour
    {
        set { colour = value; }
    }

    public float NodeSize
    {
        get { return nodeSize; }
        set { nodeSize = value; }
    }
}
#endif
