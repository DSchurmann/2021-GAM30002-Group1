using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Vector3 spawnPoint;

    private Quaternion spawnRotation;

    public bool UseStartPositionAsSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (UseStartPositionAsSpawnPoint)
        {
            spawnPoint = transform.position;
            spawnRotation = transform.rotation;
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
            Respawn();
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
    }    
}
