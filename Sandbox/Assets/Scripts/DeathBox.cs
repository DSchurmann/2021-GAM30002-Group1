using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerControllerRB child = other.gameObject.GetComponent<ChildControllerRB>();
        if (child != null)
        {
            GameController.GH.LoadGame();
        }
    }
}
