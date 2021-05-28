using UnityEngine;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint left;
    [SerializeField] private Waypoint right;
    [SerializeField] private CameraWaypoint camWaypoint;

    private void Start()
    {
        //GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (left)
            Debug.DrawLine(transform.position, left.transform.position);
        if (right)
            Debug.DrawLine(transform.position, right.transform.position);
    }

    public Waypoint Left
    {
        get { return left; }
    }
    
    public Waypoint Right
    {
        get { return right; }
    }

    public CameraWaypoint CameraWaypoint
    {
        get { return camWaypoint; }
    }


}
