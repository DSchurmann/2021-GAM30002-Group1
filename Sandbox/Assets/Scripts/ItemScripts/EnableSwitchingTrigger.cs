using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSwitchingTrigger : MonoBehaviour, ITriggeredObject
{

    public float delay;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleSwitching()
    {
        GameController.GH.IsFriend = true;
        GameController.GH.UH.disableUI = false;

        Debug.Log("Enable switching");
        Destroy(gameObject);
    }

    public void Trigger(bool value, int item)
    {
        Invoke(nameof(ToggleSwitching), delay);
    }

}
