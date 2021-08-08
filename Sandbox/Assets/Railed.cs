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

        /*workVector = GetComponent<Train>().MoveX(force);
        if (workVector.sqrMagnitude > 0.1f) { _rb.velocity = workVector; }*/

        segment = _rail.GetSegmentOfClosestPoint(_tr.position, 0.1f);
        //Vector3 targetPos = _rail.CatmullMove(segment, _rail.ClosestPointOnCatmullRomAsPercent(_tr.position, segment, 0.1f));
        Vector3 targetPos = _rail.ClosestPointOnCatmullRom(_tr.position, segment, 0.1f);
        //Vector3 targetPos = GetComponent<Train>().GetPos(_rail);
        //Vector3 pos = _rail.ClosestPointOnCatmullRom(_tr.position, 0.1f);
        _tr.position = new Vector3(targetPos.x, _tr.position.y, targetPos.z);


        Quaternion rot = Quaternion.LookRotation(workVector, transform.up);
        Quaternion jointRot = Quaternion.RotateTowards(transform.rotation, rot, 2);

        if (orientToPath)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 2);

        Quaternion ffVq = jointRot;
        Vector3 ffV = ffVq * Vector3.forward;
        ffV = _rail.transform.TransformVector(ffV);
        ffV.y = 0;

        // Constraint the velocity to the path direction
        Vector3 dir = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        Vector3 vel = new Vector3((Vector3.Dot(dir, ffV) * ffV).x, _rb.velocity.y, (Vector3.Dot(dir, ffV) * ffV).z);
        _rb.velocity = vel;


        _rb.AddForce(new Vector3(ffV.x, 0, ffV.z), ForceMode.VelocityChange);


        //GetComponent<Rigidbody>().velocity = Vector3.Dot(GetComponent<Rigidbody>().velocity, ffV) * ffV;
        //GetComponent<Rigidbody>().AddForce(new Vector3((ffV * force).x, GetComponent<Rigidbody>().velocity.y, (ffV * force).z));



        /*Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        Train train = GetComponent<Train>();

        if (train != null)
            position = train.MoveX(0);*/

    }
}
