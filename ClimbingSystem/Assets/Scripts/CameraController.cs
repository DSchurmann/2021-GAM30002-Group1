using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform lookTarget;
    public float maxDistance_x;
    public float maxDistance_y;
    public float maxDistance_z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(lookTarget);

        Vector3 target = lookTarget.position;
        //target.y += 2;
        target.x += maxDistance_x;
        target.y += maxDistance_y;
        target.z += maxDistance_z;

        //transform.DOLookAt(target, 0.5f);

        transform.position = target;

       /* float dist = Vector3.Distance(target, transform.position);

        if ((target.x - transform.position.x) > maxDistance_x)
        {
            transform.DOMoveX(lookTarget.position.x, 2f);
        }

        if ((target.x - transform.position.x) < -maxDistance_x)
        {
            transform.DOMoveX(lookTarget.position.x, 2f);
        }

        if ((target.y - transform.position.y) > maxDistance_y)
        {
            transform.DOMoveY(lookTarget.position.y, 2f);
        }

        if ((target.y - transform.position.y) < -maxDistance_y)
        {
            transform.DOMoveY(lookTarget.position.y, 2f);
        }*/
    }
}
