using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteInEditMode]
public class LedgeDetector
{
    // public components
    public Transform player;
    public Collider collider;

    public bool ledgeFound;
    public Vector3 ledgePosition;

    public float[] originForward;
    public float[] originDown;
    public float lengthForward = 0.5f;
    public float lengthDown = 2;


    public LedgeDetector(Transform player, float[] originsForward, float[] originsDown)
    {
        // get componenets
        this.player = player;
        collider = player.GetComponent<Collider>();

        originForward = originsForward;
        originDown = originsDown;
    }

    public bool HeightCheck(Vector3 origin, float height)
    {
        RaycastHit heightHit;
        if(Physics.Raycast(origin, Vector3.up, out heightHit, height))
        {
            Debug.DrawLine(ledgePosition, heightHit.point, Color.red);
            //Debug.Log(heightHit.collider.gameObject.name);
            return false;
        }
        Debug.DrawLine(ledgePosition, ledgePosition + Vector3.up * height, Color.cyan);
        return true;
    }

    public bool[] TouchingWall()
    {
        // create hits array
        bool[] hits = new bool[originForward.Length];
        RaycastHit[] racastHitsForward = new RaycastHit[originForward.Length];

        // create the rays
        List<Ray> raysForward = new List<Ray>();

        for (int i = 0; i < originForward.Length; i++)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray();
            float rayLength = lengthForward;
            ray.origin = player.position + Vector3.up * originForward[i];
            ray.direction = player.forward * lengthForward;
            raysForward.Add(ray);

            // add hit to array
            hits[i] = Physics.Raycast(ray, out hit, rayLength);
            racastHitsForward[i] = hit;
            // draw ray result
            if (hits[i] == true)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);
            }

        }

        // if hit wall
        if (hits.Length > 0)
        {
            bool wallVault = hits[0] && !hits[1] && !hits[2];
            bool wallClimb = hits[1] && !hits[2];
            bool wallJumpClimb = hits[2];


            if (!wallVault && !wallClimb && !wallJumpClimb)
            {
                ledgeFound = false;
                ledgePosition = Vector3.zero;
            }

            //Debug.Log(ColourConsoleText(canVault, "VAULT") + "    " + ColourConsoleText(canClimb, "CLIMB") + "    " + ColourConsoleText(canJumpClimb, "JUMP"));


            // check wall vault hit
            if (wallVault)
            {
                List<Ray> raysDown = new List<Ray>();

                RaycastHit hit;
                Ray ray = new Ray();
                float rayLength = lengthDown;
                ray.origin = raysForward[1].origin + raysForward[1].direction * lengthForward;
                //ray.origin = raysForward[1].origin + raysForward[1].direction * lengthForward + (Vector3.up * originDown[0]);
                ray.direction = -player.up * lengthDown;
                raysDown.Add(ray);

                bool heightCheck = Physics.Raycast(ray, out hit, rayLength);

                if (heightCheck)
                {
                    ledgeFound = true;
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    //Debug.DrawRay(racastHitsForward[0].point, hit.point, Color.cyan);
                    Vector3 pos1 = racastHitsForward[0].point;
                    Vector3 pos2 = hit.point;

                    pos1.y = pos2.y;

                    Vector3 EdgePoint = new Vector3(pos1.x, pos2.y, pos1.z);
                    ledgePosition = EdgePoint;
                    Debug.DrawLine(EdgePoint, hit.point, Color.blue);
                    Debug.DrawLine(racastHitsForward[0].point, pos1, Color.blue);
                }
                else
                {
                    ledgeFound = false;
                    ledgePosition = Vector3.zero;
                }

                //Debug.DrawRay(raysForward[1].origin + raysForward[1].direction * lengthForward, Vector3.down * lengthDown, Color.red);
            }

            // check wall climb hit
            if (wallClimb)
            {
                List<Ray> raysDown = new List<Ray>();

                RaycastHit hit;
                Ray ray = new Ray();
                float rayLength = lengthDown;
                //ray.origin = raysForward[1].origin + raysForward[1].direction * lengthForward;
                ray.origin = raysForward[2].origin + raysForward[2].direction * lengthForward;
                ray.direction = -player.up * lengthDown;
                raysDown.Add(ray);

                bool heightCheck = Physics.Raycast(ray, out hit, rayLength);

                if (heightCheck)
                {
                    ledgeFound = true;
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    //Debug.DrawRay(racastHitsForward[0].point, hit.point, Color.cyan);
                    Vector3 pos1 = racastHitsForward[1].point;
                    Vector3 pos2 = hit.point;

                    pos1.y = pos2.y;

                    Vector3 EdgePoint = new Vector3(pos1.x, pos2.y, pos1.z);
                    ledgePosition = EdgePoint;
                    Debug.DrawLine(EdgePoint, hit.point, Color.cyan);
                    Debug.DrawLine(racastHitsForward[1].point, pos1, Color.cyan);
                }
                else
                {
                    ledgeFound = false;
                    ledgePosition = Vector3.zero;
                }

                //Debug.DrawRay(raysForward[1].origin + raysForward[1].direction * lengthForward, Vector3.down * lengthDown, Color.red);
            }

            // check wall jump climb hit
            if (wallJumpClimb)
            {
                List<Ray> raysDown = new List<Ray>();

                RaycastHit hit;
                Ray ray = new Ray();
                float rayLength = lengthDown;
                //ray.origin = raysForward[2].origin + raysForward[2].direction * lengthForward;
                ray.origin = raysForward[2].origin + raysForward[2].direction * lengthForward + (Vector3.up * originDown[0]);
                ray.direction = -player.up * lengthDown;
                raysDown.Add(ray);

                bool heightCheck = Physics.Raycast(ray, out hit, rayLength);

                if (heightCheck)
                {
                    ledgeFound = true;
                    //Debug.DrawLine(ray.origin, hit.point, Color.red);
                    //Debug.DrawRay(racastHitsForward[0].point, hit.point, Color.cyan);
                    Vector3 pos1 = racastHitsForward[2].point;
                    Vector3 pos2 = hit.point;

                    pos1.y = pos2.y;

                    Vector3 EdgePoint = new Vector3(pos1.x, pos2.y, pos1.z);
                    ledgePosition = EdgePoint;
                    Debug.DrawLine(EdgePoint, hit.point, Color.white);
                    Debug.DrawLine(racastHitsForward[2].point, pos1, Color.white);
                    
                }
                else
                {
                    ledgeFound = false;
                    ledgePosition = Vector3.zero;
                }

                //Debug.DrawRay(raysForward[1].origin + raysForward[1].direction * lengthForward, Vector3.down * lengthDown, Color.red);
            }
        }
        return hits;
    }

    string ColourConsoleText(bool result, string text)
    {
        string newString = "";
        if (result)
        {
            newString = string.Format("<color=#00ff00>{0}</color>", text);
        }
        else
        {
            newString = string.Format("<color=#ff0000>{0}</color>", text);
        }

        return newString;
    }


}
