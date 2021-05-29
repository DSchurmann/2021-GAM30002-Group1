using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Train : MonoBehaviour
{
    private const float TRAIN_INCREMENT_AMOUNT = 0.1f;

    [SerializeField] private float moveSpeed = 10f;
    private float jumpHeight = 7.5f;
    [SerializeField] private int segment = 3;
    [SerializeField] private float d = 0;
    [SerializeField] private Rail rail;

    private float y = 0;
    private CharacterController cc;

    public float percentage;
    public bool flip = true;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(!rail)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        ////handle jumping
        ///*if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
        //{
        //    y = jumpHeight;
        //
        //}
        //else if (cc.isGrounded)
        //{
        //    y = Physics.gravity.y / 4f;
        //}
        //else
        //{
        //    y += Physics.gravity.y * Time.deltaTime;
        //}*/
        //
        ////float input = Input.GetAxis("Horizontal");
        //Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        ////get catmull move
        ////get percentage of the distance of the player in the current segment
        //float dist = Vector3.Distance(rail.GetNodePos(segment), pos);
        //percentage = dist / startDist;
        //
        //if (!flip)
        //{
        //    Debug.Log("Going fowards");
        //    d = percentage + TRAIN_INCREMENT_AMOUNT;
        //}
        //else
        //{
        //    Debug.Log("Going backwards");
        //    d = percentage - TRAIN_INCREMENT_AMOUNT;
        //}
        //
        //if (d > 0.975f)
        //{
        //    Debug.Log("should be changing somehow");
        //    if (segment + 1 < rail.NodeLength - 1)
        //    {
        //        percentage = 0;
        //        segment++;
        //        startDist = Vector3.Distance(pos, rail.GetNodePos(segment + 1));
        //    }
        //    else
        //    {
        //        flip = !flip;
        //    }
        //}
        //else if (d < 0.025f)
        //{
        //    if (segment > 0)
        //    {
        //        percentage = 1;
        //        segment--;
        //        startDist = Vector3.Distance(pos, rail.GetNodePos(segment));
        //    }
        //    else
        //    {
        //        flip = !flip;
        //    }
        //}
        //
        //
        ////move
        //bool goodMove = false;
        //Vector3 catmullP = rail.CatmullMove(segment, percentage);
        //Vector3 catmullD = catmullP;
        //while (!goodMove)
        //{
        //    catmullD = rail.CatmullMove(segment, d);
        //    float testDist = Vector3.Distance(catmullD, catmullP);
        //    Debug.Log(testDist);
        //    Debug.Log((TRAIN_INCREMENT_AMOUNT + TRAIN_INCREMENT_AMOUNT * startDist) / 1.8);
        //    //if (testDist < (TRAIN_INCREMENT_AMOUNT + TRAIN_INCREMENT_AMOUNT * startDist) / 1.8)
        //    if ((catmullD - pos).magnitude < moveSpeed/70)
        //    {
        //        Debug.Log("bad dist");
        //        if (flip)
        //        {
        //            d -= TRAIN_INCREMENT_AMOUNT;
        //            if (d < 0.025f)
        //            {
        //                if (segment > 0)
        //                {
        //                    percentage = 1;
        //                    segment--;
        //                    startDist = Vector3.Distance(pos, rail.GetNodePos(segment));
        //                    d = percentage - TRAIN_INCREMENT_AMOUNT;
        //                    d = percentage + TRAIN_INCREMENT_AMOUNT;
        //                }
        //                else
        //                {
        //                    flip = !flip;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            d += TRAIN_INCREMENT_AMOUNT;
        //            if (d > 0.975f)
        //            {
        //                if (segment + 1 < rail.NodeLength - 1)
        //                {
        //                    percentage = 0;
        //                    segment++;
        //                    startDist = Vector3.Distance(pos, rail.GetNodePos(segment + 1));
        //                    d = percentage + TRAIN_INCREMENT_AMOUNT;
        //                }
        //                else
        //                {
        //                    flip = !flip;
        //                    d = percentage - TRAIN_INCREMENT_AMOUNT;
        //                }
        //            }
        //        }
        //        Debug.Log(d);
        //    }
        //    else
        //    {
        //        goodMove = true;
        //    }
        //}
        //
        //
        //
        //Debug.DrawLine(catmullD, pos, new Color(0f, 0f, 1.0f), 1, false);
        //Debug.DrawLine(catmullP, pos, new Color(0f, 1.0f, 0f), 1, false);
        //Debug.DrawLine(catmullD, catmullP, new Color(1.0f, 0f, 0f), 1, false);
        //Vector3 offset = catmullD - pos;
        //offset = offset.normalized * moveSpeed;
        //
        //Vector3 rot = rail.Rotate(segment, 1);
        //Vector3 newRot = new Vector3(rot.x, transform.position.y, rot.z);
        //transform.LookAt(newRot);
        //
        //Vector3 move = new Vector3(0, y, 0);
        //
        ///*if (Input.GetAxis("Horizontal") != 0)
        //{
        //   
        //}*/
        //move = new Vector3(offset.x, y, offset.z);
        ////move = Vector3.forward * Input.GetAxis("Horizontal");
        //
        //cc.Move(move * Time.deltaTime);



        // get the position of the train along the segment

        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        

        //get catmull move
        //get percentage of the distance of the player in the current segment
        percentage = rail.ClosestPointOnCatmullRom(pos, segment);
        Vector3 catmullP = rail.CatmullMove(segment, percentage);

        Debug.DrawLine(catmullP, pos, Color.red);
        Debug.DrawLine(transform.position, pos, Color.red);
        Debug.DrawLine(catmullP, transform.position, Color.red);

        // move the target
        float targetPercent = flip ? percentage - 0.1f : percentage + 0.1f;

        if (targetPercent > 1.0f)
        {
            if (rail.NodeLength - 1 == segment + 1)
            {
                flip = !flip;
                targetPercent = 1.0f;
            }
            else
            {
                segment += 1;
                targetPercent -= 1;
            }
        }
        else if (targetPercent < 0.0f)
        {
            if (0 == segment)
            {
                flip = !flip;
                targetPercent = 0.0f;
            }
            else
            {
                segment -= 1;
                targetPercent += 1;
            }
        }

        Vector3 targetPos = rail.CatmullMove(segment, targetPercent);
        Debug.DrawLine(catmullP, targetPos, Color.green);
        Vector3 offset = targetPos - pos;
        offset = offset.normalized * moveSpeed;
        
        Vector3 rot = rail.Rotate(segment, 1);
        Vector3 newRot = new Vector3(rot.x, transform.position.y, rot.z);
        transform.LookAt(newRot);
        
        Vector3 move = new Vector3(0, y, 0);
        
        move = new Vector3(offset.x, y, offset.z);
        
        cc.Move(move * Time.deltaTime);
    }
}
