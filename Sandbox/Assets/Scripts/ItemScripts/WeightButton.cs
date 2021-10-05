using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeightButton : MonoBehaviour, ITrigger
{
    public bool enable;
    public bool triggered;
    private bool triggeredCutscene;
    // value needed to trigger something
    public float triggerValue;
    // platform idle position
    private Vector3 idlePos;
    private float idlePosY;
    [SerializeField] private CameraFollow camera;
   
    // triggered object positions positions
    [Header("Triggered Object")]
    public GameObject[] triggeredObjects;
    //private ITriggeredObject toTrigger;
    //private float buttonIdlePos;
    //private float buttonTriggeredPos;
    // button mode
    public Cutscene cutsceneToTrigger;
    public enum TriggerMode { ONCE, HOLD}
    public TriggerMode triggerMode;
    
    // Start is called before the first frame update
    void Start()
    {
        //triggerMode = TriggerMode.ONCE;
        idlePos = transform.position;
        idlePosY = transform.position.y;
        //objectIdlePos = triggeredObject.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
            CheckForPress();
    }

    public void ConstrainHeight()
    {
        if(transform.position.y > idlePosY)
        {
            Vector3 pos = transform.position;
            pos.y = idlePosY;
            GetComponent<Rigidbody>().position = idlePos;
        }
    }

    // check press value againt trigger value
    void CheckForPress()
    {
        float diff = idlePosY - transform.position.y;

        if (diff >= triggerValue)
        {
            // check switch mode
            switch(triggerMode)
            {
                case TriggerMode.ONCE:
                    if (!triggered)
                    {
                        triggered = true;
                        Shake();
                        SendTrigger();
                    }
                    break;

                case TriggerMode.HOLD:
                    triggered = true;
                    Shake();
                    SendTrigger();
                    break;
            }
        }
        else
        {
            if(triggered)
            {
                triggered = false;
                SendTriggerReset();
            }
        }
    }

    public void SendTrigger()
    {
        if(cutsceneToTrigger != null && !triggeredCutscene)
        {
            GameController.GH.GetComponent<Director>().StartCutscene(cutsceneToTrigger);
        }
        foreach (GameObject item in triggeredObjects)
        {
            if (item != null)
                item.GetComponent<ITriggeredObject>()?.Trigger(true);
        }
    }
    public void SendTriggerReset()
    {
        foreach (GameObject item in triggeredObjects)
        {
            if(item != null)
                item.GetComponent<ITriggeredObject>()?.Trigger(false);
        }
    }

    private void Shake()
    {
        camera.Shake(5, 5);
    }
}
