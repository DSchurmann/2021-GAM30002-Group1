using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSwitchingTrigger : MonoBehaviour, ITriggeredObject
{
    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleSwitching()
    {
        GameController.GH.IsFriend = !GameController.GH.IsFriend;
    }

    public void Trigger(bool value)
    {
        ToggleSwitching();
        Debug.Log("Enable switching");
        Destroy(gameObject);
    }

}
