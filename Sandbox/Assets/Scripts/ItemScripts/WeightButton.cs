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
    private CameraFollow camera;

    public float cameraShakeIntestity = 2f;
    public float cameraShakeTime = 2f;
    public bool cameraShakeOnRelease = true;
   
    // triggered object positions positions
    [Header("Triggered Object")]
    public GameObject[] triggeredObjects;

    public int positionIndex = 0;
    //private ITriggeredObject toTrigger;
    //private float buttonIdlePos;
    //private float buttonTriggeredPos;
    // button mode
    public Cutscene cutsceneToTrigger;
    public enum TriggerMode { ONCE, HOLD, MULTIPLE }
    public TriggerMode triggerMode;
    
    // Start is called before the first frame update
    void Start()
    {
        //triggerMode = TriggerMode.ONCE;
        idlePos = transform.position;
        idlePosY = transform.position.y;
        //objectIdlePos = triggeredObject.position.y;
        camera = GameObject.Find("CameraFollow").GetComponent<CameraFollow>();
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
                case TriggerMode.MULTIPLE:
                case TriggerMode.ONCE:
                    if (!triggered)
                    {
                        triggered = true;
                        Shake();
                        PlayPressSound();
                        SendTrigger();
                    }
                    break;

                case TriggerMode.HOLD:              
                    if (!triggered)
                    {
                        PlayPressSound();
                        Shake();
                    }
                       
                    SendTrigger();
                    triggered = true;
                    break;

            }
        }
        else
        {
            if(triggered)
            {
                switch (triggerMode)
                {
                    case TriggerMode.MULTIPLE:
                        if(triggered)
                        { 
                            //PlayPressSound();
                            triggered = false;
                        }
                        
                       
                        break;
                    case TriggerMode.ONCE:
                        //PlayPressSound();
                        break;

                    case TriggerMode.HOLD:
                        if (triggered)
                        {
                            //PlayPressSound();
                            triggered = false;
                        }
                        SendTriggerReset();
                        if (cameraShakeOnRelease)
                        {
                            Shake();
                        }
                        break;
                }
            }
        }
    }

    public void SendTrigger()
    {
        if(cutsceneToTrigger != null && !triggeredCutscene)
        {
            GameController.GH.GetComponent<Director>().StartCutscene(cutsceneToTrigger);
            triggeredCutscene = true;
        }
        foreach (GameObject item in triggeredObjects)
        {
            if (item != null)
                item.GetComponent<ITriggeredObject>()?.Trigger(true, positionIndex);
        }
    }
    public void SendTriggerReset()
    {
        foreach (GameObject item in triggeredObjects)
        {
            if(item != null)
                item.GetComponent<ITriggeredObject>()?.Trigger(false, positionIndex);
        }
    }

    private void Shake()
    {
        camera.Shake(cameraShakeIntestity, cameraShakeTime);
    }

    public void PlayPressSound()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
    }
}
