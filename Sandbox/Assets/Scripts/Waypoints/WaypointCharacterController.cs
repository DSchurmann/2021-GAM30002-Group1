using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WaypointCharacterController : MonoBehaviour
{
    private CharacterController cc;
    private float maxSpeed = 0.5f;
    [SerializeField] private Waypoint currentWaypoint;
    [SerializeField] private WaypointCameraController cam;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cam.Waypoint = currentWaypoint.CameraWaypoint;
    }

    private void Update()
    {
        Vector3 moveTarget;
        float hor = Input.GetAxis("Horizontal");
        if(hor < 0) //left
        {
            if (currentWaypoint.Left != null)
            {
                moveTarget = currentWaypoint.Left.transform.position - transform.position;
                cc.Move(moveTarget * Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.Left.transform.position, maxSpeed * Time.deltaTime);
                //create look rotation for looking at the target (child)
                Quaternion lookRot = Quaternion.LookRotation(currentWaypoint.Left.transform.position - transform.position);
                //get only the y rotation
                Quaternion newRot = Quaternion.Euler(0f, lookRot.eulerAngles.y, 0f);
                //change the object rotation with Slerp (spherical linear interpolation, thanks Google), this rotates the object gradually, or interpolates, instead of snapping to a rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 1f);

                ChangeWaypoint();
            }
        }
        else if(hor > 0) //right
        {
            if (currentWaypoint.Right != null)
            {
                moveTarget = currentWaypoint.Right.transform.position - transform.position;
                cc.Move(moveTarget * Time.deltaTime);

                //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.Right.transform.position, maxSpeed * Time.deltaTime);
                //create look rotation for looking at the target (child)
                Quaternion lookRot = Quaternion.LookRotation(currentWaypoint.Right.transform.position - transform.position);
                //get only the y rotation
                Quaternion newRot = Quaternion.Euler(0f, lookRot.eulerAngles.y, 0f);
                //change the object rotation with Slerp (spherical linear interpolation, thanks Google), this rotates the object gradually, or interpolates, instead of snapping to a rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 1f);

                ChangeWaypoint();
            }
        }



    }

    private void ChangeWaypoint()
    {
        float dist = 1f;
        if (currentWaypoint.Left != null)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.Left.transform.position) < dist)
            {
                currentWaypoint = currentWaypoint.Left;
                cam.Waypoint = currentWaypoint.CameraWaypoint;
            }
        }

        if (currentWaypoint.Right != null)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.Right.transform.position) < dist)
            {
                currentWaypoint = currentWaypoint.Right;
                cam.Waypoint = currentWaypoint.CameraWaypoint;
            }
        }
    }
}