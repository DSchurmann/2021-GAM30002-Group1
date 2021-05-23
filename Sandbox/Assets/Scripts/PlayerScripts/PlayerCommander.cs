using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : MonoBehaviour
{
    public AIController golem;

    private bool riding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("ISSUED COMMAND E");
            if(!riding)
                NavigateTo(transform.position);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("ISSUED COMMAND F");
            if(!riding)
                ToggleFollow();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("ISSUED COMMAND R");
            float feetDistance = Vector3.Distance(transform.position, golem.transform.position);
            float headDistance = Vector3.Distance(transform.position, golem.Head.transform.position);

            if (feetDistance < 2 || headDistance < 2)
            {
                ToggleRide();
            }
        }
    }

    public string ToggleRide()
    {
        string result = "none";

        if(!riding)
        {
            // disable and move player character
            GetComponent<PlayerAnimationController>().SetJumping(false);
            GetComponent<CharacterController>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerMove>().enabled = false;
            transform.GetComponent<Animator>().enabled = false;
            transform.position = golem.Shoulder.position;
            transform.parent = golem.Shoulder;
            transform.forward = golem.transform.forward;
            // enable golem move scripts
            golem.gameObject.GetComponent<PlayerController>().enabled = true;
            golem.gameObject.GetComponent<PlayerMove>().enabled = true;
            golem.following = false;
            golem.DisableNav();
            riding = true;
        }
        else
        {
            // disable golem move scripts
            golem.gameObject.GetComponent<PlayerController>().enabled = false;
            golem.gameObject.GetComponent<PlayerMove>().enabled = false;
            // enable and move player character
            GetComponent<CharacterController>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerMove>().enabled = true;
            transform.GetComponent<Animator>().enabled = true;
            //transform.position = golem.Shoulder.forward * 2;
            transform.parent = null;
          
            golem.StopFollowing();
            golem.following = false;
            golem.EnableNav();
            riding = false;
        }
       

        return result;
    }

    public string NavigateTo(Vector3 position)
    {
        string result = "none";

        golem.SetNavTarget(position);

        return result;
    }
    
    public string ToggleFollow()
    {
        string result = "none";

        if(golem.following)
        {
            golem.StopFollowing();
        }
        else
        {
            golem.following = true;
        }

        return result;
    }
}
