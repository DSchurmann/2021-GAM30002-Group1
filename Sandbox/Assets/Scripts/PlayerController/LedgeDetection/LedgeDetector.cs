using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LedgeDetector : MonoBehaviour
{
    // public components
    protected ChildControllerRB player;
    public Collider collider;
    // public bools
    public bool detectLedges;
    public bool EdgeFound;
    // public Vectors
    public Vector3 EdgePoint;
    public Vector3 TargetPosition;
    // public ledge types
    public bool Vaultable;
    public float vaultDistance;

    //public variablies
    [SerializeField]
    public float lengthDown = 1;
    // private player variables
    private bool isTouchingWall;
    private bool isLedgeFound;


    private void Start()
    {
        // get componenets
        player = GetComponent<ChildControllerRB>();
        collider = GetComponent<Collider>();
        // get player variables
        isTouchingWall = player.CheckTouchingWall();
    }

    private void Update()
    {
        DetectLedges();

        // check for ledges if player touching wall
        if (isTouchingWall)
        {
            DetectLedges();
        }

    }

    private void DetectLedges()
    {
        if (detectLedges)
        {
            float lengthForward = player.wallCheckDistance;
            Vector3 originForward = player.wallCheck.position;
            Vector3 dirForward = Vector3.zero;
            Vector3 originDown1 = Vector3.zero;
            Vector3 originDown2 = Vector3.zero;


            // set forward direction from player facing direction
            if (player.FacingDirection == 0)
            {
                dirForward = Vector3.right;
            }
            else
            {
                dirForward = Vector3.right * player.FacingDirection;
            }
            // sat raycast variables
            RaycastHit hitForward;
            RaycastHit hitDown;
            RaycastHit hitDownVaultCheck;

            //Debug.DrawRay(player.wallCheck.position + new Vector3(0,-0.5f,0), Vector3.right * (player.FacingDirection * player.wallCheckDistance), Color.blue);
            //Debug.DrawLine(originForward, dirForward * lengthForward);
            if (Physics.Raycast(originForward, dirForward, out hitForward, lengthForward))
            {
                originDown1 = new Vector3(hitForward.point.x, hitForward.point.y + 2, hitForward.point.z);
                originDown2 = player.wallCheck.position + player.transform.TransformDirection(new Vector3(0.0f, collider.bounds.size.y + 0.25f, vaultDistance));

                Debug.DrawRay(player.wallCheck.position, Vector3.right * (player.FacingDirection * player.wallCheckDistance), Color.blue);
                if (Physics.Raycast(originDown1, Vector3.down, out hitDown, lengthDown))
                {
                    //Debug.DrawLine(originForward, hitForward.point, Color.red);
                    //Debug.DrawLine(originDown, hitDown.point, Color.red);
                    Debug.DrawRay(originDown1, Vector3.down * lengthDown, Color.blue);
                    Vector3 pos1 = hitForward.point;
                    Vector3 pos2 = hitDown.point;
                    TargetPosition = hitDown.point;

                    pos1.y = pos2.y;

                    EdgePoint = new Vector3(pos1.x, pos2.y, pos1.z);
                    EdgeFound = true;

                    //Debug.DrawLine(EdgePoint, hitDown.point, Color.red);
                    //Debug.DrawLine(hitForward.point, EdgePoint, Color.red);

                   /* if (Physics.Raycast(originDown2, Vector3.down, out hitDownVaultCheck, lengthDown))
                    {
                        if ((hitDown.point.y - hitDownVaultCheck.point.y) < 0.5f)
                        {
                            Debug.DrawLine(originDown2, hitDownVaultCheck.point, Color.red);
                            Vaultable = false;
                        }
                        else
                        {
                            Debug.DrawLine(originDown2, hitDownVaultCheck.point, Color.green);
                            Vaultable = true;
                        }
                    }
                    else
                    {
                        Vaultable = false;
                    }*/
                }
                else
                {
                    EdgePoint = Vector3.zero;
                    EdgeFound = false;
                }
            }
            else
            {
                EdgePoint = Vector3.zero;
                EdgeFound = false;
            }
        }
    }
}
