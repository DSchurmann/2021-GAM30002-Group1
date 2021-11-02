using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebuggerCheckpointSkip : MonoBehaviour
{
    private Vector3 initialGolemPos;
    private Rail initalGolemRail;
    private Vector3 initialChildPos;
    private Rail initalChildRail;

    private int index = 0;

    // Start is called before the first frame update
    void Awake()
    {
//#if UNITY_EDITOR
        enabled = true;
/*#else
        enabled = false;
#endif*/
    }

    private void Update()
    {
        if(Keyboard.current.equalsKey.wasPressedThisFrame)
        {
            NextCheckpoint();
        }

        if (Keyboard.current.minusKey.wasPressedThisFrame)
        {
            PreviousCheckpoint();
        }
    }

    void Start()
    {
        initialGolemPos = GameController.GH.golemObj.transform.position;
        //initalGolemRail = GameController.GH.golemObj.Train.rail;
        initialChildPos = GameController.GH.childObj.transform.position;
        //initalChildRail = GameController.GH.childObj.Train.rail;
    }

    public void NextCheckpoint()
    {
        index++;
        if (index > GetCheckpoints().Length)
            index = 0;

        SetPositions(index - 1);

        GameController.GH.IsFriend = true;
    }

    public void PreviousCheckpoint()
    {
        index--;
        if (index < 0)
            index = GetCheckpoints().Length;

        SetPositions(index - 1);

        GameController.GH.IsFriend = true;
    }

    private Checkpoint[] GetCheckpoints()
    {
        return FindObjectsOfType<Checkpoint>();
    }

    private void SetPositions(int i)
    {
        if (i == -1)
        {
            GameController.GH.golemObj.transform.position = initialGolemPos;
            //GameController.GH.golemObj.Train.rail = initalGolemRail;
            GameController.GH.childObj.transform.position = initialChildPos;
            //GameController.GH.childObj.Train.rail = initalChildRail;
        }
        else
        {
            Checkpoint checkpoint = GetCheckpoints()[i];

            GameObject[] railObjects = GameObject.FindGameObjectsWithTag("Rail");
            // find search for clostest rail to target

            float bestDistance = float.PositiveInfinity;
            Rail closestRail = GameController.GH.childObj.Train.rail; // set the rail to the current rail as default

            foreach (GameObject railObject in railObjects)
            {
                Rail r = railObject.GetComponent<Rail>();

                if (r.RailType == RailType.Golem)
                    continue;

                float dist = r.DistanceToClosestPoint(checkpoint.transform.position);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closestRail = r;
                }
            }

            // set the player posistion
            GameController.GH.childObj.Train.rail = closestRail;
            GameController.GH.childObj.Train.segment = closestRail.GetSegmentOfClosestPoint(checkpoint.transform.position);
            GameController.GH.childObj.transform.position = new Vector3(closestRail.ClosestPointOnCatmullRom(checkpoint.transform.position).x, checkpoint.transform.position.y, closestRail.ClosestPointOnCatmullRom(checkpoint.transform.position).z);


            bestDistance = float.PositiveInfinity;
            closestRail = GameController.GH.golemObj.Train.rail; // set the rail to the current rail as default

            foreach (GameObject railObject in railObjects)
            {
                Rail r = railObject.GetComponent<Rail>();

                if (r.RailType == RailType.Child)
                    continue;

                float dist = r.DistanceToClosestPoint(checkpoint.transform.position);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closestRail = r;
                }
            }

            // set the player posistion
            GameController.GH.golemObj.Train.rail = closestRail;
            GameController.GH.golemObj.Train.segment = closestRail.GetSegmentOfClosestPoint(checkpoint.transform.position);
            GameController.GH.golemObj.transform.position = new Vector3(closestRail.ClosestPointOnCatmullRom(checkpoint.transform.position).x, checkpoint.transform.position.y, closestRail.ClosestPointOnCatmullRom(checkpoint.transform.position).z);
        }
    }
}
