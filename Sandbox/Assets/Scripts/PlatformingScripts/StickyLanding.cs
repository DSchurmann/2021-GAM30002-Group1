 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyLanding : MonoBehaviour
{
    private MovingPlatform platform = null;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         if(collision.gameObject.GetComponent<Rigidbody>().velocity.y <=0)
         {
             parent = collision.gameObject.transform.parent;
             collision.gameObject.transform.parent = this.transform;
         }
     }*/

    private void FixedUpdate()
    {
        if(platform != null)
        {
            if (parent != null)
            {
                transform.parent = parent;
            }
            else
            {
                // default; no parent
                transform.parent = null;
            }
            platform = null;
        }

        if (Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, out RaycastHit hit, 2))
        {
            if (hit.collider.gameObject.GetComponent<MovingPlatform>() != null)
            {
                if (platform == null)
                {
                    //Debug.Log("GOLEM ON PLATFORM");

                    platform = hit.collider.GetComponent<MovingPlatform>();
                    parent = transform.parent;
                    transform.parent = platform.transform;
                }
            }
        }
    }

    void AddPlatform()
    {

    }

    void RemovePlatform()
    {

    }

    void OnCollisionStay(Collision collisionInfo)
    {

        //if (collisionInfo.gameObject == GameController.GH.childObj)
        if (collisionInfo.gameObject.GetComponent<MovingPlatform>() != null)
        {
            
        }
    }
}
