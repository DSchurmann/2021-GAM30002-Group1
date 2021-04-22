using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LedgeDetector : MonoBehaviour
{
    public bool detectLedges;
    public bool EdgeFound;

    public Vector3 EdgePoint;
    public Vector3 AheadPoint;

    public Vector3 leftHandEdge;
    public Vector3 rightHandEdge;

    public Collider characterCollider;

    public float forwardCastLength;
    public float downwardCastLength;

    public RaycastHit forwardHit;
    public RaycastHit downHit;

    // Start is called before the first frame update
    void Start()
    {
        characterCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        DetectLedges();

        /*if(edgeFound)
        {
            Debug.DrawLine(transform.position + transform.TransformDirection(new Vector3(0.0f, 1.5f, 0.0f)), EdgePoint, Color.cyan);
        } */
    }


    private void DetectLedges()
    {
        if (detectLedges)
        {
            float lengthForward = forwardCastLength;
            float lengthDown = downwardCastLength;

            Vector3 originForward = transform.position + transform.TransformDirection(new Vector3(0.0f, 0.5f, 0.0f));
            Vector3 dirForward = transform.TransformDirection(Vector3.forward);

            Vector3 dirDown = Vector3.down;

            RaycastHit hitForward;
            RaycastHit hitDown;


            if (Physics.Raycast(originForward, dirForward, out hitForward, lengthForward))
            {
                Vector3 originDown = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y + 0.25f, lengthForward));
                forwardHit = hitForward;


                if (Physics.Raycast(originDown, dirDown, out hitDown, lengthDown))
                {
                    Debug.DrawLine(originForward, hitForward.point, Color.red);
                    Debug.DrawLine(originDown, hitDown.point, Color.red);

                    Vector3 pos1 = hitForward.point;
                    Vector3 pos2 = hitDown.point;
                    pos1.y = pos2.y;

                    EdgePoint = new Vector3(pos1.x, pos2.y, pos1.z);
                    EdgeFound = true;
                    
                    Debug.DrawLine(EdgePoint, hitDown.point, Color.red);
                    Debug.DrawLine(hitForward.point, EdgePoint, Color.red);
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
