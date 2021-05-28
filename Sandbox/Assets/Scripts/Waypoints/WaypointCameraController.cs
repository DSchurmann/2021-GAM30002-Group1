using UnityEngine;

public class WaypointCameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public CameraWaypoint waypoint;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, waypoint.transform.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoint.transform.position, 3f);
        }
        else
        {
            transform.rotation = waypoint.transform.rotation;
        }
    }

    public CameraWaypoint Waypoint
    {
        set { waypoint = value; }
    }
}