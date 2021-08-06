using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railed : MonoBehaviour
{
    private int segment = -1;
    public Rail _rail;
    private Transform _tr;
    private Rigidbody _rb;
    

    public bool orientToPath = true;
   
    public int force = 0;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (_rail == null)
            return;

        float minPos = ComputePosAtMinDistance();
        _tr.position = GetPositionForPos(minPos);

    }

    private float ComputePosAtMinDistance()
    {
        if (_rail == null)
            return 0f;

        float minPos = 0f;


        return minPos;
    }

    private Vector3 GetPositionForPos(float pos)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        return position;
    }

}
