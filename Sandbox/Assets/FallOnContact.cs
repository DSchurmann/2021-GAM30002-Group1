using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnContact : MonoBehaviour
{

    public Rigidbody[] RBs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerControllerRB>() != null)
        {
            foreach (var item in RBs)
            {
                item.isKinematic = false;
            }
            //GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
