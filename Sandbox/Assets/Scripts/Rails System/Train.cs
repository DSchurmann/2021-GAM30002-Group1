using UnityEngine;
using System.Linq;


public class Train : MonoBehaviour
{
    // the distance to move along the rail in each step
    private const float TRAIN_INCREMENT_DIST = 0.6f;

    public int segment = -1;
    public Rail rail;
    [SerializeField] private bool isConnectedtoRail = false;
    private Vector2 dir;

    [SerializeField] private float railSeekRange = 2f;
    [SerializeField] private RailType type;

    public float RailSeekRange { get { return railSeekRange; } }


    private float percentage;

    [SerializeField, Range(0.125f, 0.925f)] private float DeadZone = 0.7f;
    int dirInput = 0;

    private void Start()
    {
        dir = Vector2.right;
    }

    public Vector3 GetPos(Rail rail)
    {
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        segment = rail.GetSegmentOfClosestPoint(pos, 0.1f);
        return rail.ClosestPointOnCatmullRom(pos, 0.1f);
        //return MoveX(0);
    }


    public Vector3 MoveX(float velocityX)
    {
        // invert path
        //if (isConnectedtoRail && rail.swapControls)
        //{
        //    Debug.Log("swap");
        //    float temp = velocityX;
        //    velocityX = VelocityZ;
        //    VelocityZ = temp;
        //}


        // get the position of the train
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        
        GameObject[] railObjects = GameObject.FindGameObjectsWithTag("Rail");

        GetRail(pos, railObjects);

        
        // if still not connected to a rail move in direction dir
        if (!isConnectedtoRail)
        {
            return dir.normalized * velocityX;
        }

        JumpRail(pos, railObjects);

        // get percentage of the distance of the player in the current segment
        percentage = rail.ClosestPointOnCatmullRomAsPercent(pos, segment);
        // get the position of the train along the segment
        Vector3 catmullP = rail.CatmullMove(segment, percentage);

        // debug in editor
        Debug.DrawLine(catmullP, pos, Color.red);
        Debug.DrawLine(transform.position, pos, Color.red);
        Debug.DrawLine(catmullP, transform.position, Color.red);

        // Calculate the percentage to increment, so that the amount is the same in all segments
        float dist = rail.GetSegmentLength(segment);
        float incremenmtAmount = TRAIN_INCREMENT_DIST / dist;

        // move the target
        float targetPercentage = percentage + Mathf.Sign(velocityX) * incremenmtAmount;

        if (targetPercentage > 1.0f)
        {
            if (rail.NodeLength - 1 == segment + 1)
            {
                // at the end of the rail disconnect
                targetPercentage = 1.0f;
                foreach (GameObject railObject in railObjects)
                {
                    Rail r = railObject.GetComponent<Rail>();
                    if (r == rail || !CheckType(r))
                        continue;
                    // rail is within range
                    if (r.IsRailWithinRange(pos, railSeekRange))
                    {
                        rail = r;
                        segment = rail.GetSegmentOfClosestPoint(pos);
                        isConnectedtoRail = true;
                        break;
                    }
                    else
                    {
                        isConnectedtoRail = false;
                    }
                }
            }
            else
            {
                segment += 1;
                targetPercentage -= 1;
            }
        }
        else if (targetPercentage < 0.0f)
        {
            if (0 == segment)
            {
                // at the start of the rail disconnect
                targetPercentage = 0.0f;
                foreach (GameObject railObject in railObjects)
                {
                    Rail r = railObject.GetComponent<Rail>();
                    if (r == rail || !CheckType(r))
                        continue;
                    // rail is within range
                    if (r.IsRailWithinRange(pos, railSeekRange))
                    {

                        rail = r;
                        segment = rail.GetSegmentOfClosestPoint(pos);
                        isConnectedtoRail = true;
                        break;
                    }
                    else
                    {
                        isConnectedtoRail = false;
                    }
                }
            }
            else
            {
                segment -= 1;
                targetPercentage = 1;
            }
        }

        //segment = rail.GetSegmentOfClosestPoint(pos);
        Vector3 targetPos = rail.CatmullMove(segment, targetPercentage);       
        Debug.DrawLine(catmullP, targetPos, Color.green);
        Vector3 offset = targetPos - pos;
        offset = offset.normalized * Mathf.Abs(velocityX);

        Vector3 move = new Vector3(offset.x, 0, offset.z);

        return move;
    }

    // get rail
    public void GetRail(Vector3 playerPosition, GameObject[] railObjects)
    {
        // check if player is not currenly connected to a rail
        if (!isConnectedtoRail)
        {
            foreach (GameObject railObject in railObjects)
            {
                Rail r = railObject.GetComponent<Rail>();
                if (!CheckType(r))
                    continue;
                // rail is within range
                if (r.IsRailWithinRange(playerPosition, railSeekRange, false))
                {
                    //Debug.Log("connect to rail path");
                    rail = r;
                    segment = rail.GetSegmentOfClosestPoint(playerPosition);
                    isConnectedtoRail = true;
                    break;
                }
            }  
        }
    }

    // jump rail
    public void JumpRail(Vector3 playerPosition, GameObject[] railObjects)
    {
        if (GetComponent<PlayerControllerRB>() != null)
        {
            float inputY =  GetComponent<PlayerControllerRB>().InputHandler.RawMovementInput.y;
            if (Mathf.Abs(inputY) >= DeadZone && GetComponent<PlayerControllerRB>().ControllerEnabled)
            {
                dirInput = (int)Mathf.Sign(inputY);
            }
            else
            {
                dirInput = 0;
            }
        }

        // jump rails
        foreach (GameObject railObject in railObjects)
        {
            Rail r = railObject.GetComponent<Rail>();

            // skip if refering to ourselves of the rail has a lower Priority
            if (r == rail || r.Priority < rail.Priority || !CheckType(r))
                continue;

            if (r.Priority > rail.Priority && CheckType(r))
            {
                //Debug.Log(r.name + " - " + CheckType(r));
                // rail is within range
                if (r.IsRailWithinRange(playerPosition, railSeekRange, false))
                {
                    rail = r;
                    segment = rail.GetSegmentOfClosestPoint(playerPosition);
                    break;
                }
            }
            else if (dirInput != 0)
            {
                //Debug.Log("this is where it changes");
                // rail is within range
                if (r.IsRailWithinRange(playerPosition, railSeekRange))
                {
                    // use dot product to ensure key press is in the right direction
                    float dot = Vector3.Dot(Vector3.forward * dirInput, r.ClosestPointOnCatmullRom(playerPosition) - playerPosition);
                    //Debug.Log("DOT: " + dot);
                    //Debug.Log("DIR INPUT: " + dirInput);
                    //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;//Use arc cosine to get the radian of the angle and convert it into an angle
                    //Debug.Log(string.Format("Angle:{0}", angle));

                    if (dot > 0)
                    {
                        rail = r;
                        segment = rail.GetSegmentOfClosestPoint(playerPosition);
                        break;
                    }
                }
            }
        }
    }
    
    private bool CheckType(Rail r)
    {
        if(r.Type == type)
        {
            return true;
        }
        else if(r.Type == RailType.Both)
        {
            return true;
        }
        return false;
    }
}