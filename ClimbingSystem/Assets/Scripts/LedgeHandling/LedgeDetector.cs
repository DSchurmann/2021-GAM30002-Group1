using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LedgeDetector : MonoBehaviour
{
    public bool detectLedges;
    public bool EdgeFound;

    public Vector3 EdgePoint;
    public Vector3 TargetPosition;

    public bool Vaultable;
    public float vaultDistance;
    public Collider characterCollider;
    public float forwardCastLength;
    public float downwardCastLength;
    public RaycastHit forwardHit;
    public RaycastHit downHit;

    private List<Ray> raycasts = new List<Ray>();


    // Start is called before the first frame update
    void Start()
    {
        characterCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
   /* void Update()
    {
       
    }*/

    void Update()
    {
        //DetectLedges();
        DetectLedges2();
    }


    private void DetectLedges2()
    {
        float lengthForward = forwardCastLength;
        float lengthUp = characterCollider.bounds.size.y;
        float height = 0.2f;

        Vector3 origin1 = transform.position + transform.TransformDirection(new Vector3(0.0f, height, 0.0f));
        Ray ray1 = new Ray(origin1, transform.forward);

        Vector3 origin2 = ray1.origin + ray1.direction;
        Ray ray2 = new Ray(ray1.origin + (ray1.direction * lengthForward), transform.up);



        // a cast
        RaycastHit aHit;
        RaycastHit bHit;

        bool a = Physics.Raycast(ray2, out aHit, lengthUp);

        Debug.DrawLine(ray2.origin, ray2.origin + ray2.direction * lengthUp, Color.cyan);

        if (a)
        {
            Debug.DrawLine(ray1.origin, ray1.origin + ray1.direction * lengthForward, Color.blue);
           /* bool b = Physics.Raycast(ray2, out bHit, lengthUp);

            if(b)
            {
                Debug.DrawLine(ray2.origin, ray2.origin + ray2.direction * lengthUp, Color.blue);
            }*/
        }
            




       
        /* if (*//* you want to find the top of the wall *//*)
         {
             rayStart = hit1 + transform.forward * 0.1; // Arbitrary number that will be "into" the wall
             rayStart.y += 10.0f; // Arbitrary number high enough to be above the wall
             if (Physics.Raycast(rayStart, Vector3.down, out hit2, ...)
             {
                 Edge = new Vector3(hit1.x, hit2.y, hit1.z);
                 if (*//* you can reach the top of the ledge *//*)
                 {
                     // Grab the ledge and calculate other handhold points here
                 }
             }
         }*/

    }
    public void DrawRays(Ray ray, float length, Color colour)
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * length);
    }



   /* private void DetectLedges()
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
            RaycastHit hitDownVaultCheck;


            if (Physics.Raycast(originForward, dirForward, out hitForward, lengthForward))
            {
                Vector3 originDown = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y + 0.25f, lengthForward));
                Vector3 originDown2 = transform.position + transform.TransformDirection(new Vector3(0.0f, characterCollider.bounds.size.y + 0.25f, vaultDistance));
                forwardHit = hitForward;


                if (Physics.Raycast(originDown, dirDown, out hitDown, lengthDown))
                {
                    Debug.DrawLine(originForward, hitForward.point, Color.red);
                    Debug.DrawLine(originDown, hitDown.point, Color.red);

                    Vector3 pos1 = hitForward.point;
                    Vector3 pos2 = hitDown.point;
                    TargetPosition = hitDown.point;

                    pos1.y = pos2.y;

                    EdgePoint = new Vector3(pos1.x, pos2.y, pos1.z);
                    EdgeFound = true;
                    
                    Debug.DrawLine(EdgePoint, hitDown.point, Color.red);
                    Debug.DrawLine(hitForward.point, EdgePoint, Color.red);

                    if (Physics.Raycast(originDown2, dirDown, out hitDownVaultCheck, lengthDown))
                    {
                        if ((hitDown.point.y - hitDownVaultCheck.point.y) <0.5f)
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
                    }
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
    }*/
}
