using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSwitchingTrigger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleSwitching()
    {
        GameController.GH.CanSwitch = !GameController.GH.CanSwitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        ToggleSwitching();
        Debug.Log("Enable switching");
        Destroy(gameObject);
    }

}
