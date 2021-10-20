using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollisionDetector : MonoBehaviour
{
    private MovingPlatform platform;
    private Collider col;

    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponentInParent<MovingPlatform>();
        col = GetComponent<Collider>();

        //col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DetectCharacterHit(Collider hit)
    {
        if (hit.GetComponent<GolemControllerRB>() != null || hit.GetComponentInParent<GolemControllerRB>() != null)
        {
            Debug.Log("HIT GOLEM");

            platform.hold = true;

        }
        else if (hit.GetComponent<ChildControllerRB>() != null)
        {
            Debug.Log("HIT CHILD");
            hit.GetComponent<ChildControllerRB>().ChangeState(hit.GetComponent<ChildControllerRB>().InstantDeathState);
        }
    }

    public void DetectCharacterGone(Collider hit)
    {
        if (hit.GetComponent<GolemControllerRB>() != null || hit.GetComponentInParent<GolemControllerRB>() != null)
        {
            platform.hold = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(platform.isFalling || platform.isMoving)
        {
            if (other.GetComponent<PlayerControllerRB>() != null || other.GetComponentInParent<PlayerControllerRB>() != null)
                DetectCharacterHit(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (platform.isFalling || platform.isMoving)
        {
            if (other.GetComponent<PlayerControllerRB>() != null || other.GetComponentInParent<PlayerControllerRB>() != null)
                DetectCharacterGone(other);
        }
    }
}
