using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    private Vector3 spawnPosition;
    private GameObject curCheckPoint;

    void Start()
    {
        spawnPosition = transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            respawn();
        }
    }

    void respawn()
    {
        transform.position = spawnPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //if (curCheckPoint != other)
            {
                spawnPosition = other.transform.position;
                //curCheckPoint = other.transform.parent.gameObject;
            }
        }
    }
}
