using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool enabled;
    public Vector3 startPos;
    public Vector3 endPos;

    public float moveSpeed;

    public float waitTime;
    private bool waiting;
    private bool waited;
    private float waitTimer;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if(transform.position == targetPos)
            {
                if(!waiting)
                    waiting = true;
            }

            if (transform.position == startPos)
            {
                targetPos = endPos;
            }
            else if (transform.position == endPos)
            {
                targetPos = startPos;
            }

            if(!waiting)
                MoveToPosition(transform, targetPos, moveSpeed);

            if(waiting)
            {
                waitTimer += Time.deltaTime;
                {
                    if((waitTimer%60) > waitTime)
                    {
                        waiting = false;
                        waitTimer = 0;
                    }
                }
            }
        }
    }

    IEnumerator WaitFor(float waitTime)
    {
           yield return new WaitForSeconds(waitTime);    
    }

    public void MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, timeToMove * Time.deltaTime);
    }

    public void SetEndPosition()
    {
        endPos = transform.position;
    }
}
