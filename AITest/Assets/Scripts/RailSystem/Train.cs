using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    private int segment = 0;
    private float d;
    [SerializeField] private Rail rail;

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
        d += Time.deltaTime * (Input.GetAxis("Horizontal") / moveSpeed);
        if(d < 0)
        {
            dir = 1;
            segment--;
        }
        else if(d > 1)
        {
            dir = 0 ;
            segment++;
        }

        transform.position = rail.Move(segment, d);
    }
}
