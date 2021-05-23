using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public float pickupRange;
    private float pickupAngleMin = 0.7f;

    public GameObject targetObj;
    float minLookAtObjectAngle = 45;


    private GameObject carryObj;
    public bool carrying;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dot;
        float dist;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (carrying)
            {
                PutDownObject(carryObj);
            }
            else
            // if facing object pick it up
            {
                Transform[] pickupableObjects = FindPickupsInRange(pickupRange);
                Transform pickup;

                if (pickupableObjects.Length > 0)
                {
                    pickup = UtilityFunctions.GetClosestByTransform(transform.position, pickupableObjects);

                    dot = Vector3.Dot(transform.forward, (pickup.transform.position - transform.position).normalized);
                    dist = Vector3.Distance(transform.position, pickup.transform.position);

                    Debug.Log("Item: " + pickup.gameObject.name + "   Distance: " + Vector3.Distance(transform.position, pickup.position));

                    if (dot > pickupAngleMin && dist < pickupRange)
                    {
                        Debug.Log("picking up object");
                        PickupObject(pickup.gameObject);
                    }
                }
                else
                {
                    Debug.Log("Nothing to pickup in range");
                }
            }
            
           


            /* if(pickupableObjects.Length >0)
             {
                 foreach (var item in pickupableObjects)
                 {
                     Debug.Log("Item: " + item.gameObject.name + "   Distance: " + Vector3.Distance(transform.position,item.position));
                 }
             }
             else
             {
                 Debug.Log("Nothing to pickup in range");
             }*/

            /* // put down object if carrying, else pickup object
             if(carrying)
             {
                 PutDownObject(carryObj);
             }
             else
             // if facing object pick it up
             {
                 if (dot > 0.7f && dist < 2f)
                 {
                     Debug.Log("picking up object");
                     PickupObject(targetObj);
                 }
             }*/

        }

       

    }
    void PickupObject(GameObject obj)
    {
        carryObj = obj;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.gameObject.transform.parent = gameObject.transform;
        Vector3 carryPos = obj.transform.position;
        carryPos.y += 1.1f;
        carryPos.x = 0.0f;
        carryPos.z = 0.8f;
        obj.transform.localPosition= carryPos;
        carrying = true;
        GetComponent<PlayerAnimationController>().SetCarrying(true);
    }

    void PutDownObject(GameObject obj)
    {
        obj.gameObject.transform.parent = null;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        carrying = false;
        GetComponent<PlayerAnimationController>().SetCarrying(false);
    }

    Transform[] FindPickupsInRange(float radius)
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Pickups_Child");
        float dist = 0;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.forward, 1f, mask);

        Transform[] pickups = new Transform[hits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            pickups[i] = hits[i].transform;
        }

        return pickups;
    }

}
