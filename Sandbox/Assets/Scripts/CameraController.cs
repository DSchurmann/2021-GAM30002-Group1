using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform lookTarget;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(lookTarget);

        transform.DOLookAt(lookTarget.position, 0.5f);

        float dist = Vector3.Distance(lookTarget.position, transform.position);

        if ((lookTarget.position.x - transform.position.x) > maxDistance)
        {
            transform.DOMoveX(lookTarget.position.x, 2f);
        }

        if ((lookTarget.position.x - transform.position.x) < -maxDistance)
        {
            transform.DOMoveX(lookTarget.position.x, 2f);
        }
    }
}
