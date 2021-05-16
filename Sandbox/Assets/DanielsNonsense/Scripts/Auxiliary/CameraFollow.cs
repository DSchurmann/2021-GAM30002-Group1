using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Target Object
    public GameObject targObj;
    public float movSpeed;

    public float distance;

    //Start
    private void Start()
    {
        movSpeed = (7f);
        //targObj = GameHandler.GH.childObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targObj.transform.position) > 1.6f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targObj.transform.position, ((movSpeed) * Time.deltaTime));
        }
    }
}
