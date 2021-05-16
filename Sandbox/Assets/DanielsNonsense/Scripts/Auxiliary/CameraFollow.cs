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
    }

    // Update is called once per frame
    void Update()
    {
        //Set Target Object
        if (GameHandler.GH.switchMode)
            targObj = GameHandler.GH.golemObj;
        else
            targObj = GameHandler.GH.childObj;

        //If Distance
       /* if (Vector3.Distance(transform.position, targObj.transform.position) > 0.1f && Vector3.Distance(transform.position, targObj.transform.position) < 3f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targObj.transform.position, ((movSpeed / 2) * Time.deltaTime));
        }
        else */
        if (Vector3.Distance(transform.position, targObj.transform.position) > 1.6f)
        {
            //Go go go
            transform.position = Vector3.MoveTowards(transform.position, targObj.transform.position, ((movSpeed) * Time.deltaTime));
        }
    }
}
