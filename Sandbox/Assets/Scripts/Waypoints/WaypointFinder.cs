using UnityEngine;

[ExecuteInEditMode]
public class WaypointFinder : MonoBehaviour
{
    public Waypoint[] paths;

    [SerializeField] private Waypoint left;
    [SerializeField] private Waypoint right;
    [SerializeField] private Waypoint up;
    [SerializeField] private Waypoint down;

    private void Start()
    {
        
    }

    private void Update()
    {
        /*if (left)
            Debug.DrawLine(transform.position, left.transform.position);
        if (right)
            Debug.DrawLine(transform.position, right.transform.position);*/
    }

    public Waypoint Left
    {
        get { return left; }
    }
    
    public Waypoint Right
    {
        get { return right; }
    }

    public Waypoint Up
    {
        get { return up; }
    }

    public Waypoint Down
    {
        get { return down; }
    }


}
