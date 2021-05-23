using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint left;
    [SerializeField] private Waypoint right;
    [SerializeField] private CameraWaypoint camWaypoint;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
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
