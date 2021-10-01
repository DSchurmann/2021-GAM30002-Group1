using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMe : MonoBehaviour
{

    public int dir;
    public float speed;
    public float angle = 15;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Quaternion targetRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y + (angle * dir), transform.rotation.z);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);

    }
}
