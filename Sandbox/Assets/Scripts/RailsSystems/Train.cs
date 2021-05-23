using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Train : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private float jumpHeight = 7.5f;
    [SerializeField] private int segment = 0;
    [SerializeField] private float d = 0;
    [SerializeField] private Rail rail;

    [SerializeField] private float startDist;
    private float y = 0;
    private CharacterController cc;

    public float percentage;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        startDist = Vector3.Distance(transform.position, rail.GetNodePos(segment + 1));
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
        //handle jumping
        /*if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            y = jumpHeight;

        }
        else if (cc.isGrounded)
        {
            y = Physics.gravity.y / 4f;
        }
        else
        {
            y += Physics.gravity.y * Time.deltaTime;
        }*/

        //float input = Input.GetAxis("Horizontal");
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        //get catmull move
        //get percentage of the distance of the player in the current segment
        float dist = Vector3.Distance(rail.GetNodePos(segment), pos);
        percentage = dist / startDist;

        d = percentage + (1 * 0.1f);
        if (percentage > 1 && d > 1)
        {
            if (segment < rail.NodeLength - 2)
            {
                percentage = 0;
                segment++;
                startDist = Vector3.Distance(pos, rail.GetNodePos(segment + 1));
            }
        }
        else if (percentage < 0.06 && d < 0)
        {
            if (segment > 0)
            {
                percentage = 1;
                segment--;
                startDist = Vector3.Distance(pos, rail.GetNodePos(segment));
            }
        }

        //move
        Vector3 catmullD = rail.CatmullMove(segment, d);
        Vector3 catmullP = rail.CatmullMove(segment, percentage);
        Vector3 offset = catmullD - pos;

        offset = offset.normalized * moveSpeed;

        Vector3 rot = rail.Rotate(segment, 1);
        Vector3 newRot = new Vector3(rot.x, transform.position.y, rot.z);
        transform.LookAt(newRot);

        Vector3 move = new Vector3(0, y, 0);

        /*if (Input.GetAxis("Horizontal") != 0)
        {
           
        }*/
        move = new Vector3(offset.x, y, offset.z);
        //move = Vector3.forward * Input.GetAxis("Horizontal");

        cc.Move(move * Time.deltaTime);
    }
}
