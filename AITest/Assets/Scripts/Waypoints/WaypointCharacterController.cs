using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WaypointCharacterController : MonoBehaviour
{
    private CharacterController cc;
    private float maxSpeed = 5f;
    [SerializeField] private Waypoint currentWaypoint;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        if(hor < 0) //left
        {
            if (currentWaypoint.Left != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.Left.transform.position, maxSpeed * Time.deltaTime);
                ChangeWaypoint();
            }
        }
        else if(hor > 0) //right
        {
            if (currentWaypoint.Right != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.Right.transform.position, maxSpeed * Time.deltaTime);
                ChangeWaypoint();
            }
        }
        
    }

    private void ChangeWaypoint()
    {
        if (currentWaypoint.Left != null)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.Left.transform.position) < 0.3f)
            {
                currentWaypoint = currentWaypoint.Left;
            }
        }

        if (currentWaypoint.Right != null)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.Right.transform.position) < 0.3f)
            {
                Debug.Log("change should happen to: " + currentWaypoint.Right.name);
                currentWaypoint = currentWaypoint.Right;
            }
        }
    }
}