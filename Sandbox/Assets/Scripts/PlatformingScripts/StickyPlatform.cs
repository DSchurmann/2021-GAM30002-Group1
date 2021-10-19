 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{

    private Transform parent;
    private GameObject player = null;

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
    

        if(player != null)
        {
            if (parent != null)
            {
                player.transform.parent = parent;
            }
            else
            {
                player.transform.parent = null;
            }
            player = null;
        }
    }

    void AddPlayer()
    {

    }

    void RemovePlayer()
    {

    }

    void OnCollisionStay(Collision collisionInfo)
    {

        //if (collisionInfo.gameObject == GameController.GH.childObj)
        if (collisionInfo.gameObject.GetComponent<ChildControllerRB>() != null)
        {
            //Debug.Log("PLAYER ON PLATFORM");
            if (Physics.Raycast(collisionInfo.gameObject.transform.position, Vector3.down, out RaycastHit hit, 2))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    if(player == null)
                    {
                        Debug.Log("CHILD ON PLATFORM");
                        parent = collisionInfo.gameObject.transform.parent;
                        collisionInfo.gameObject.transform.parent = this.transform;
                        player = collisionInfo.gameObject;
                    }
                }
            }
        }
        else if (collisionInfo.gameObject.GetComponent<GolemControllerRB>() != null)
        {
            //Debug.Log("PLAYER ON PLATFORM");
            if (Physics.Raycast(collisionInfo.gameObject.transform.position, Vector3.down, out RaycastHit hit, 2))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    if (player == null)
                    {
                        Debug.Log("GOLEM ON PLATFORM");
                        parent = collisionInfo.gameObject.transform.parent;
                        collisionInfo.gameObject.transform.parent = this.transform;
                        player = collisionInfo.gameObject;
                    }
                }
            }
        }
    }
}
