using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Cutscene cutsceneToTrigger;

    public enum TriggeredBy { Child, Golem, Both}

    public TriggeredBy triggeredBy = TriggeredBy.Both;

    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            switch(triggeredBy)
            {
                case TriggeredBy.Both:
                    if (other.GetComponent<PlayerControllerRB>() != null || (other.transform.parent != null && other.GetComponentInParent<PlayerControllerRB>() != null))
                    {
                        GameController.GH.GetComponent<Director>().StartCutscene(cutsceneToTrigger);
                        triggered = true;
                    }
                    break;

                case TriggeredBy.Child:
                    if (other.GetComponent<ChildControllerRB>() != null)
                    {
                        GameController.GH.GetComponent<Director>().StartCutscene(cutsceneToTrigger);
                        triggered = true;
                    }
                    break;

                case TriggeredBy.Golem:
                    if (other.GetComponentInParent<GolemControllerRB>() != null)
                    {
                        GameController.GH.GetComponent<Director>().StartCutscene(cutsceneToTrigger);
                        triggered = true;
                    }
                    break;
            }
        }
    }
}
