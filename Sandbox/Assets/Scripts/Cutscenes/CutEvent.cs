using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutEvent: MonoBehaviour
{
    //Parameters -- Core
    public string eventName;
    public enum EventType { moveChild, moveGolem, moveBoth, targetChild, targetGolem, lookAt, animChild, animGolem, changeCamera, rotateCamera, fadeIn, fadeOut, changeScene };
    public EventType eventType;
    public bool synchronous; //Does this event immediately hit up the next event? Or do we only continue when this one is finished?

    //Parameters -- Movement
    public Vector3 newPos;

    //Parameters -- Movement
    public Transform targetObject;

    //Parameters -- Animation to play
    public string animName;
    public float animBlendTime = 0;

    //Parameters -- Camera / Other Rotation Change
    public Vector3 newRotation;

    //Parameters -- New Scene
    public int sceneNo;

    //Parameters -- Duration?
    public float duration; //(In Seconds)

    //Constructor
    public CutEvent(string name, EventType type, bool sync, float length)
    {
        //Set Parameters 
        eventName = name;
        eventType = type;
        synchronous = sync;
        duration = length;
    }

    //Constructor -- With Position & Rotation
    public CutEvent(string name, EventType type, bool sync, Vector3 targetPos, Vector3 targetRot, float length)
    {
        //Set Parameters 
        eventName = name;
        eventType = type;
        synchronous = sync;
        newPos = targetPos;
        newRotation = targetRot;
        duration = length;
    }

    //Constructor -- With Position & Rotation & target
    public CutEvent(string name, EventType type, bool sync, Vector3 targetPos, Transform targetObj, Vector3 targetRot, float length)
    {
        //Set Parameters 
        eventName = name;
        eventType = type;
        synchronous = sync;
        newPos = targetPos;
        targetObject = targetObj;
        newRotation = targetRot;
        duration = length;
    }

    //Constructor -- Change of Scene
    public CutEvent(string name, EventType type, bool sync, int newScene)
    {
        //Set Parameters 
        eventName = name;
        eventType = type;
        synchronous = sync;
        sceneNo = newScene;
    }

    //Constructor -- PlayAnimation
    public CutEvent(string name, EventType type, bool sync, string anim, float length)
    {
        //Set Parameters 
        eventName = name;
        eventType = type;
        synchronous = sync;
        animName = anim;
        duration = length;
    }
}
