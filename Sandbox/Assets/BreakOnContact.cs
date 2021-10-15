using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnContact : MonoBehaviour
{
    public GameObject ToDestroy;
    public GameObject[] DestroyedVersionParts;


    void Break()
    {
        //Instantiate(DestroyedVersion, transform.position, transform.rotation);


        Destroy(ToDestroy.gameObject);

        foreach (var item in DestroyedVersionParts)
        {
            item.SetActive(true);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ChildControllerRB>() != null)
        {
            Break();
            other.gameObject.GetComponent<ChildControllerRB>().ChangeState(other.gameObject.GetComponent<ChildControllerRB>().FallState);
        }
    }
}
