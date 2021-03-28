using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float camSpeed;

    [SerializeField] private Transform child;
    [SerializeField] private Transform golem;
    private Transform target;

    private void Start()
    {
        target = child;
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.55f);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
