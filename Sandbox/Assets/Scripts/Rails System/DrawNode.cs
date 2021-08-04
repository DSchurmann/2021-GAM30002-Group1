using UnityEditor;
using UnityEngine;

public class DrawNode: MonoBehaviour
{
    private Color colour;
    [SerializeField] private float nodeSize = 1f;
    
    private void Start()
    {
        SceneView.RepaintAll();
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = colour;
        Gizmos.DrawCube(transform.position, Vector3.one * nodeSize);
#endif
    }

    public Color SetColour
    {
        set { colour = value; }
    }

    public float SetNodeSize
    {
        set { nodeSize = value; }
    }
}
