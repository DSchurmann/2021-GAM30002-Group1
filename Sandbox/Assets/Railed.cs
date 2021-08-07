using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railed : MonoBehaviour
{
    private int segment = -1;
    public Rail _rail;
    private Transform _tr;
    private Rigidbody _rb;

    private Vector3 workVector;

    public bool orientToPath = true;
   
    public float force = 0;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (_rail == null)
            return;

        workVector = GetComponent<Train>().MoveX(force);

       _rb.velocity = workVector;

        /*Vector3 ffV = transform.rotation * Vector3.forward;
        //ffV = pathCreator.transform.TransformVector(ffV);
        ffV.y = 0;
        GetComponent<Rigidbody>().velocity = Vector3.Dot(GetComponent<Rigidbody>().velocity, ffV) * ffV;

        GetComponent<Rigidbody>().AddForce(new Vector3((ffV * force).x, GetComponent<Rigidbody>().velocity.y, (ffV * force).z));*/


        /*Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;


        Train train = GetComponent<Train>();

        if (train != null)
            position = train.MoveX(0);*/

    }
}
