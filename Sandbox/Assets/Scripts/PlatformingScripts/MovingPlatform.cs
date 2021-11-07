using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class MovingPlatform : MonoBehaviour, ITriggeredObject
{
    public bool enabled = true;
    private bool loop = true;
    private bool triggeredEnabled = false;
    private bool triggered = false;
    public bool hold;
    private bool held;
    private int clicks;
    private bool colliding;
    private bool CombinedTrigger0;
    private bool CombinedTrigger1;

    private Queue<int> targetIndexes = new Queue<int>();

    public bool isMoving;
    public bool isFalling;
    // platform modes
    public enum PlatFormMode { NONE, PINGPONG, ONCE, HOLDOPEN, MULTIPLE }
    public PlatFormMode platformMode = PlatFormMode.NONE;
    // trigger modes
    public enum TriggerMode { NONE, ONCE, HOLD, TOGGLE, MULTIPLE, COMBINED }
    public TriggerMode triggerMode = TriggerMode.NONE;

    // start and end positions
    public Vector3 startPos = Vector3.zero;
    public List<Vector3> platformPositions = new List<Vector3>();
    public Vector3 endPos = Vector3.zero;

    [Header("Movement")]
    // move speed
    [Range(0,10f)]
    public float moveSpeed = 2;
    [Range(0, 10f)]
    public float fallSpeed = 2;
   
    [Header("Wait Time")]
    // wait time
    [Range(0, 10f)]
    public float waitTime;
    private bool waiting;
    private Vector3 targetPos;
    private Vector3 TemptargetPos;

    private bool soundPlayed;

    private bool triggered2 = false;

    [SerializeField] private bool endGameOnTrigger;

    // raycast variables
    /* private Collider collider;
     private bool hitDetect;
     private  RaycastHit hit;
     public Transform hitTransform;
     public Vector3 hitPosition;*/

    // Start is called before the first frame update
    void Start()
    {
        if (Application.IsPlaying(gameObject))
        {
            //collider = GetComponent<Collider>();

            // check if start position exists, if not, get platform position
            if (startPos == Vector3.zero)
                startPos = transform.position;

            // set platform position to start position
            transform.position = startPos;
            Loop();

            if (platformMode == PlatFormMode.MULTIPLE)
            {
                platformPositions.Insert(0, startPos);
                platformPositions.Add(endPos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

        if(transform.position == startPos || transform.position == endPos)
        {
            isMoving = false;
            isFalling = false;
            soundPlayed = false;
        }
        else if(platformMode == PlatFormMode.MULTIPLE)
        {
            foreach (Vector3 pos in platformPositions)
            {
                if (transform.position == pos)
                {
                    isMoving = false;
                    break;
                }
            }
        }

        if (enabled)
        {
            switch (platformMode)
            {
                case PlatFormMode.NONE:
                    // nothing
                    break;

                case PlatFormMode.PINGPONG:
                    PingPong();
                    break;

                case PlatFormMode.ONCE:
                    Once();
                    break;

                case PlatFormMode.HOLDOPEN:
                    HoldOpen();
                    break;
                case PlatFormMode.MULTIPLE:
                    Multiple();
                    break;
            }
        }


        /* hitDetect = Physics.BoxCast(collider.bounds.center + (Vector3.down * transform.localScale.y / 2), new Vector3(transform.localScale.x / 2, 0.2f, transform.localScale.z / 2), -transform.up, out hit, transform.rotation, 0f);
         if (hitDetect)
         {
             hitTransform = hit.collider.transform;
             hitPosition = hit.point;

             Debug.Log("HIT: " + hitTransform.gameObject.name);

             Debug.DrawLine(transform.position, hit.point, Color.cyan);
         }
         else
         {
             Debug.DrawLine(collider.bounds.center, Vector3.down * 20, Color.cyan);

             ExtDebug.DrawBoxCastBox(collider.bounds.center + (Vector3.down * transform.localScale.y / 2), new Vector3(transform.localScale.x / 2, 0.2f, transform.localScale.z / 2), transform.rotation, -transform.up, 0f, Color.cyan);
         }*/
    }

    // platform modes
    #region HOLD OPEN
    public void HoldOpen()
    {
        loop = false;
        switch (triggerMode)
        {

            case TriggerMode.HOLD:
                if (triggered)
                {
                    isFalling = false;
                    targetPos = endPos;
                    MovePlatform(moveSpeed);
                }
                else
                {
                    if(Vector3.Distance(transform.position, startPos) > 0.01f)
                    {
                        if(!isFalling)
                            soundPlayed = false;
                        Vector3 temp = startPos;
                        targetPos = startPos;
                        MovePlatform(fallSpeed);
                        isFalling = true;
                        triggered = false;
                        
                    }

                    /*if (transform.position != startPos)
                    {
                        ReturnToStart(fallSpeed);

                    }
                    else
                    {
                        //Loop();

                    }*/
                }

                break;

            case TriggerMode.COMBINED:
                if (CombinedTrigger0 && CombinedTrigger1)
                {
                    if (endGameOnTrigger)
                    {
                        if (GetComponent<SceneSwitcher>() && !triggered2)
                        {
                            triggered2 = true;
                            GetComponent<SceneSwitcher>().SwitchSceneWithFade(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                            
                    }
                    else
                    {
                        isFalling = false;
                        targetPos = endPos;
                        MovePlatform(moveSpeed);
                    }
                }
                else if (platformPositions.Count > 0 && (CombinedTrigger0 || CombinedTrigger1))
                {
                    isFalling = false;
                    targetPos = platformPositions[0];
                    MovePlatform(moveSpeed);
                }
                else
                {
                    if (Vector3.Distance(transform.position, startPos) > 0.01f)
                    {
                        if (!isFalling)
                            soundPlayed = false;
                        Vector3 temp = startPos;
                        targetPos = startPos;
                        MovePlatform(fallSpeed);
                        isFalling = true;
                        triggered = false;

                    }
                }

                break;
        }
    }
    #endregion

    #region PING PONG
    public void PingPong()
    {
        // set loop
        loop = true;
        switch (triggerMode)
        {
            case TriggerMode.NONE:
                MovePlatform(moveSpeed);
                break;
            case TriggerMode.ONCE:
                if (triggered && !triggeredEnabled)
                {
                    triggeredEnabled = true;
                }
                if (triggeredEnabled)
                    MovePlatform(moveSpeed);
                break;
            case TriggerMode.HOLD:
                if(triggered)
                {
                    MovePlatform(moveSpeed);
                }
                else
                {
                    MovePlatform(0);
                }
                break;
            case TriggerMode.MULTIPLE:
            case TriggerMode.TOGGLE:

                bool started = false;

                if (triggered && !started)
                {
                    started = true;
                }

                if (triggered && started)
                {
                    started = false;
                }
                if(started)
                {
                    MovePlatform(moveSpeed);
                }
                else
                {
                    MovePlatform(0);
                }
                    
                break;
        }
    }
    #endregion

    #region ONCE
    public void Once()
    {
        loop = false;
        switch (triggerMode)
        {
            case TriggerMode.NONE:
                MovePlatform(moveSpeed);
                break;
            case TriggerMode.ONCE:
                if (triggered && !triggeredEnabled)
                {
                    triggeredEnabled = true;
                }
                if (triggeredEnabled)
                    if(transform.position!= endPos)
                        MovePlatform(moveSpeed);
                break;

            case TriggerMode.HOLD:
                if(triggered)
                {
                    MovePlatform(moveSpeed);

                }
                else
                {
                    MovePlatform(0);
                }
               
                break;
        }
    }
    // swap direction
    public void ReturnToStart(float speed)
    {
        targetPos = startPos;
        MovePlatform(speed);
    }
    // stop platform
    public void StopPlatform()
    {
        MoveToPosition(transform, transform.position, 0);
    }

    #endregion

    #region MULTIPLE

    public void Multiple()
    {
        loop = false;
        switch (triggerMode)
        {
            case TriggerMode.MULTIPLE:

                // start movement
                if (triggered && !triggeredEnabled && targetIndexes.Count > 0)
                {
                    // start movement
                    triggered = false;
                    triggeredEnabled = true;
                    targetPos = platformPositions[targetIndexes.Dequeue()];
                }

                if (triggeredEnabled)
                {
                    if (transform.position != targetPos)
                        MovePlatform(moveSpeed);
                    else if (targetIndexes.Count > 0)
                    {
                        triggered = false;
                        targetPos = platformPositions[targetIndexes.Dequeue()];
                    }
                    else
                        triggeredEnabled = false;
                }
                break;
        }
    }

    #endregion


    // move platform
    private void MovePlatform(float speed)
    {

        // wait when reach target
        if (transform.position == targetPos && !waiting)
        {
            waiting = true;
            //StopCoroutine(MoveTo(0));
            StartCoroutine(WaitFor(waitTime));
          
        }
        // move to position if not waiting
        if (!waiting)
            //StartCoroutine(MoveTo(3));
            MoveToPosition(transform, targetPos, speed);
    }
    // wait for seconds enumerator
    IEnumerator WaitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // cancel waiting
        waiting = false;
        // loop if set
        if (loop)
            Loop();
    }

/*    private IEnumerator MoveTo(float time)
    {
        Vector3 startingPos = startPos;
        Vector3 finalPos = endPos;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }*/

    // switch direction if looping
    public void Loop()
    {
        Vector3 angle = targetPos - transform.position;


        if (transform.position == startPos)
        {
                targetPos = endPos;
        }
        else if (transform.position == endPos)
        {
                targetPos = startPos;
        }
    }


    public void MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        
            
        if (!colliding)
        {
            if (!soundPlayed)
            {
                PlaySound();
                soundPlayed = true;
            }

            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, position, timeToMove * Time.deltaTime *  Convert.ToInt32(!hold));
        }
           
    }


    public void SetStartPosition()
    {
        startPos = transform.position;
    }
    public void SetEndPosition()
    {
        endPos = transform.position;
    }
    public void setPlatformPosition()
    {
        platformPositions.Add(transform.position);
    }

    public void resetPosition()
    {
        transform.position = startPos;
    }

    public void Trigger(bool value, int item)
    {
        triggered = value;

        if (platformMode == PlatFormMode.MULTIPLE && value)
            targetIndexes.Enqueue(item);
        
        if (triggerMode == TriggerMode.COMBINED)
        {
            if (item == 1)
            {
                CombinedTrigger1 = value;
            }
            else
            {
                CombinedTrigger0 = value;
            }
        }
       /* if(triggered)
            Debug.Log("Triggered to open: " + this.GetType().Name);*/
    }

    public void PlaySound()
    {
        if(GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
    }


    private void OnCollisionEnter(Collision collision)
    {

        /*colliding = true;
        Debug.Log("Colliding");
        TemptargetPos = targetPos;
        targetPos = transform.position;*/
        //MovePlatform(0f);
    }
    private void OnCollisionExit(Collision collision)
    {
       /* colliding = false;
        Debug.Log("Not colliding");
        targetPos = TemptargetPos;*/
        //MovePlatform(moveSpeed);
    }

   
}
