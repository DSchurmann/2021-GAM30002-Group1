using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : MonoBehaviour
{

    bool isRepeatingMovement = false;
    List<Vector3> RecordedPositions;
    int recordedPositionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setRecordedPositions(List<Vector3> recordedPositions)
    {
        RecordedPositions = recordedPositions;
        isRepeatingMovement = true;
        recordedPositionIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TODO move the golem to the inial position of the player 

        // TODO follow the player when golem is not following the player of the player is not recording movement

        // follow the players movements
        if (isRepeatingMovement)
        {
            transform.position = RecordedPositions[recordedPositionIndex];
            recordedPositionIndex++;
            if (RecordedPositions.Count == recordedPositionIndex)
            {
                isRepeatingMovement = false;
            }
        }
    }
}
