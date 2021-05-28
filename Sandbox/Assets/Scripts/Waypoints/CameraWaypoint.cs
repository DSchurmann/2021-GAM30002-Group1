using UnityEngine;

public class CameraWaypoint : MonoBehaviour
{
    [SerializeField] private CameraWaypoint left;
    [SerializeField] private CameraWaypoint right;

    public CameraWaypoint Left
    {
        get { return left; }
    }

    public CameraWaypoint Right
    {
        get { return right; }
    }
}