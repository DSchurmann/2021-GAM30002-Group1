using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeightButton : MonoBehaviour
{
    public bool triggered;
    public float triggerValue;

    float idlePos;

    public Transform triggeredObject;
    public float objectIdlePos;
    public float objTriggeredPos;
    
    // Start is called before the first frame update
    void Start()
    {
        idlePos = transform.position.y;
        objectIdlePos = triggeredObject.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = idlePos - transform.position.y;
        Debug.Log(diff);
        if (diff > triggerValue)
        {
            triggered = true;
        }
        else
        {
            triggered = false;
        }

       /* if (!triggered && triggeredObject.position.y < objectIdlePos)
        {
            triggeredObject.DOMoveY(objectIdlePos, 2 * Time.deltaTime);
        }*/

        if (triggered)
        {
            if(triggeredObject.transform.position.y > objTriggeredPos)
            {
                triggeredObject.DOMoveY(objTriggeredPos,3);
                //triggeredObject.transform.position = -triggeredObject.transform.up * Time.deltaTime;
            }
        }
        else
        {
            if (triggeredObject.transform.position.y < objectIdlePos)
            {
                triggeredObject.DOMoveY(objectIdlePos, 3);
                //triggeredObject.transform.position = triggeredObject.transform.up * Time.deltaTime;
            }
        }
    }


    void TriggerObject()
    {
        triggeredObject.DOMoveY(objTriggeredPos, 2 * Time.deltaTime);
    }
}
