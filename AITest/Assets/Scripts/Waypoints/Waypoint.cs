using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint left;
    [SerializeField] private Waypoint right;

    public Waypoint Left
    {
        get { return left; }
    }
    
    public Waypoint Right
    {
        get { return right; }
    }
}
