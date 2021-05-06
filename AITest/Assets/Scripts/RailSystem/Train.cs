using UnityEngine;

public class Train : MonoBehaviour
{
    private float moveSpeed = 5f;
    private int dir;
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
        d += Time.deltaTime * 1 / 2.5f;
        Debug.Log(segment);
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

        transform.position = rail.Move(transform.position, segment, dir, d);
    }
}