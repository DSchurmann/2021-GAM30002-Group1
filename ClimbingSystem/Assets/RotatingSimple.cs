using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingSimple : MonoBehaviour
{
    public Vector3 Axis;
    public bool dir;
    public float speed;


    float angle;
    // Start is called before the first frame update
    void Start()
    {
        if (dir)
            speed = -speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Axis*speed * Time.deltaTime, Space.Self);
    }
}
