using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    private GameManager gameManager;
    private Vector3 spawnPosition;
    private GameObject curCheckPoint;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent(typeof(GameManager)) as GameManager;
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
        gameManager.Load();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //if (curCheckPoint != other)
            {
                spawnPosition = other.transform.position;
                //curCheckPoint = other.transform.parent.gameObject;

                gameManager.Save();
            }
        }
    }
}
