using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    //Singleton
    public static Director D;

    //Parameters -- Core
    public bool inCutscene;
    public Cutscene curCutscene;
    public int stage = (0);

    //Parameters -- Steps
    public Vector3 panPos;
    public Vector3 rotPos;
    public Vector3 movPos;
    public GameObject targetPos;

    //Parameters -- Components/Objects
    public GameObject fadeObj;
    public GameObject camObj;
    public GameObject widescreenBars;

    //Parameters -- Cutscene Database
    public Dictionary<string, Cutscene> cutscenes = new Dictionary<string, Cutscene>();

    //Set Singleton
    public void Awake()
    {
        //Singleton Setup
        if (D != this && D != null)
            Destroy(this.gameObject);
        else if (D == null)
            D = this;

        //Don't Destroy
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Start Sample Cutscene (for testing purposes)
        stage = (0);

        //Set Camera Object
        if (camObj == null)
            camObj = GameObject.Find("CameraFollow");

        //Initialize Cutscenes
        //InitCutscenes();

        //Run Debug Cutscene
        //StartCutscene(cutscenes["Intro"]);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeObj != null)
        {
            if (!inCutscene)
            {
                //fadeObj.SetActive(false);
                if (widescreenBars != null)
                    widescreenBars.SetActive(false);
            }
            else
            {
                //fadeObj.SetActive(true);
                if (widescreenBars != null)
                    widescreenBars.SetActive(true);
            }
        }
    }

    //Start Cutscene
    public void StartCutscene(Cutscene scene)
    {
        //Set curCutscene
        curCutscene = scene;

        //Reset Stage
        stage = (0);

        //Set InCutscene
        inCutscene = (true);

        //Begin Processing Cutscene (Coroutine)
        StartCoroutine(ProcessCutscene());
    }
    public IEnumerator ProcessCutscene()
    {
        //Get Current Stage of the Cutscene and Run It
        CutEvent cur = curCutscene.cutsceneEvents[stage];
        float progress = 0f;
        Debug.Log(cur.eventType.ToString());
        bool charMove = (cur.eventType == CutEvent.EventType.moveChild || cur.eventType == CutEvent.EventType.moveGolem || cur.eventType == CutEvent.EventType.moveBoth);
        bool camFade = (cur.eventType == CutEvent.EventType.fadeIn || cur.eventType == CutEvent.EventType.fadeOut);
        float percProgress = 0f;

        //(These should be a case statement, remember to fix)
        //Check Event Type -- ANIMATE CHILD
        switch (cur.eventType)
        {
            case CutEvent.EventType.animChild:
                //Get Child to Play Animation
                GameController.GH.childObj.GetComponent<ChildControllerRB>().GetComponent<Animator>().Play(cur.animName);
                break;

            case CutEvent.EventType.animGolem:
                //GameController.GH.golemObj.GetComponent<GolemControllerRB>().ChangeState(GameController.GH.golemObj.GetComponent<GolemControllerRB>().RaiseAbility);
                GameController.GH.golemObj.GetComponent<GolemControllerRB>().GetComponent<Animator>().Play(cur.animName);
                //GameController.GH.golemObj.GetComponent<GolemControllerRB>().ChangeState(GameController.GH.golemObj.GetComponent<GolemControllerRB>().RaiseAbility);
                break;

            case CutEvent.EventType.moveChild:
                // Move Child to position
                GameController.GH.childObj.EnableControls();
                GameController.GH.golemObj.DisableControls();
                //GameController.GH.childObj.transform.position = movPos;
                break;

            case CutEvent.EventType.moveGolem:
                // Move Golem to position
                GameController.GH.childObj.DisableControls();
                GameController.GH.golemObj.EnableControls();
                //GameController.GH.golemObj.transform.position = movPos;
                break;

            case CutEvent.EventType.moveBoth:
                // Move Golem to position
                GameController.GH.childObj.EnableControls();
                GameController.GH.golemObj.EnableControls();
                //GameController.GH.golemObj.transform.position = movPos;
                break;

            case CutEvent.EventType.changeCamera:
                //Do Camera Movement
                panPos = cur.newPos;
                //Set CameraObj Target Position
                camObj.GetComponent<CameraFollow>().PanTo(panPos);
                break;

            case CutEvent.EventType.lookAt:
                //Do Camera Movement
                camObj.GetComponent<CameraFollow>().target = cur.targetObject;
                camObj.GetComponent<CameraFollow>().followMode = CameraFollow.FollowMode.FOLLOWTARGET;
                break;

            case CutEvent.EventType.targetChild:
                //Do Camera Movement
                //panPos = GameController.GH.childObj.transform.position;
                //Set CameraObj Target Position
                GameController.GH.childObj.EnableControls();
                GameController.GH.golemObj.DisableControls();
                camObj.GetComponent<CameraFollow>().followMode = CameraFollow.FollowMode.CURRENTPLAYER;
                break;

            case CutEvent.EventType.targetGolem:
                //Do Camera Movement
                //panPos = GameController.GH.golemObj.transform.position;
                //Set CameraObj Target Position
                GameController.GH.childObj.DisableControls();
                GameController.GH.golemObj.EnableControls();
                camObj.GetComponent<CameraFollow>().followMode = CameraFollow.FollowMode.CURRENTPLAYER;
                break;

            case CutEvent.EventType.rotateCamera:
                //Do Camera Rotate
                rotPos = cur.newRotation;
                //Set CameraObj Rotation
                //camObj.GetComponent<CameraFollow>().RotateTo(rotPos);
                break;

            default:
                //Set Camera Back to Following
                if (camObj != null)
                    camObj.GetComponent<CameraFollow>().FollowToPlayer();
                break;
        }

        //Progress Animation
        do
        {
            //Set Perc
            if (cur.duration != 0f)
                percProgress = ((progress / cur.duration));
            else
                percProgress = (1f);

            //Is it an event involving moving the player or the golem?
            if (charMove)
            {
                //Set MovPos
                movPos = cur.newPos;
            }
            else
            {
                //Set MovPos
                movPos = new Vector3(0f, 0f, 0f);
            }

            //Is it a fadein/fadeout event?
            if (camFade)
            {
                //Colours
                Color startCol = new Color(0f, 0f, 0f);
                Color endCol = new Color(0f, 0f, 0f);

                //Lerp Colour
                if (cur.eventType == CutEvent.EventType.fadeIn)
                {
                    //Set Colours
                    startCol = new Color(0f, 0f, 0f, 1f);
                    endCol = new Color(0f, 0f, 0f, 0f);
                }
                else
                {
                    //Set Colours
                    startCol = new Color(0f, 0f, 0f, 0f);
                    endCol = new Color(0f, 0f, 0f, 1f);
                }

                //Do Fade
                if (fadeObj != null)
                    fadeObj.GetComponent<Image>().color = Color.Lerp(startCol, endCol, percProgress);


            }

            //On Change Scene
            if (cur.eventType == CutEvent.EventType.changeScene)
            {
                //load next scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                StopCoroutine(ProcessCutscene());
                //EndCutscene();
                break;
                //curCutscene.cutsceneEvents.Clear();

            }

            //Yield on Progress
            if (progress < cur.duration)
            {
                //Increase Progress
                progress += 0.6f * Time.deltaTime;

                //Async
                if (!cur.synchronous)
                    yield return null;
                //Sync
                //Fill up progress instantly
            }
        } while (progress < cur.duration);

        //Progress to the Next Stage, if one exists
        if (stage < curCutscene.cutsceneEvents.Count - 1)
        {
            //Increase Stage
            stage++;

            //Run again
            StartCoroutine(ProcessCutscene());
        }
        else
            EndCutscene();
    }
    public void EndCutscene()
    {
        //End Things
        curCutscene = null;

        //Reset Camera
        if (camObj != null)
            camObj.GetComponent<CameraFollow>().FollowToPlayer();

        //Reset Stage
        stage = (0);

        //Set False
        inCutscene = (false);
    }

    //Initialize Cutscenes ((This whole system will probably need to be cleaner for the finished product))
    public void InitCutscenes()
    {
        //Set Temp
        Cutscene newCut = new Cutscene("");

        //Intro2 Cutscene
        #region Intro2

        //Sample Cutscene
        //Set Name
        newCut.cutsceneName = ("Intro2");

        //Start Adding Sequences!
        //newCut.cutsceneEvents.Add(new CutEvent("PlaceChild", CutEvent.EventType.moveChild, true, new Vector3(10f, -0.5f, -3.58f), new Vector3(0f, 0f, 0f), 0f)); //Place starting point for Child
        //newCut.cutsceneEvents.Add(new CutEvent("PlaceGolem", CutEvent.EventType.moveGolem, true, new Vector3(-6.55f, 2.15f, 0f), new Vector3(0f, 0f, 0f), 0f)); //Place Starting Point for Golem
        newCut.cutsceneEvents.Add(new CutEvent("CameraFadeIn", CutEvent.EventType.fadeIn, false, .25f));
        newCut.cutsceneEvents.Add(new CutEvent("ChildWalksOnScreen", CutEvent.EventType.moveChild, false, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 0f), 1.25f)); //Child walks towards the Golem
        newCut.cutsceneEvents.Add(new CutEvent("ChildWalksOnScreen", CutEvent.EventType.moveChild, false, new Vector3(0f, 0f, 0f), new Vector3(0f, -15f, 0f), 0.5f)); //Child stops
        newCut.cutsceneEvents.Add(new CutEvent("CameraPansUp", CutEvent.EventType.changeCamera, false, new Vector3(4.95f, 0f, -2.65f), new Vector3(0f, 0f, 0f), 1f)); //Camera pans to the Golem
        newCut.cutsceneEvents.Add(new CutEvent("CameraStaysForASec", CutEvent.EventType.changeCamera, false, new Vector3(4.95f, 0f, -2.65f), new Vector3(0f, 0f, 0f), .02f)); //Camera stays in position
        newCut.cutsceneEvents.Add(new CutEvent("CameraPansUp", CutEvent.EventType.changeCamera, false, new Vector3(9.19f, -1.12f, -1.68f), new Vector3(0f, 0f, 0f), 1f)); //Camera pans to the Button
        newCut.cutsceneEvents.Add(new CutEvent("CameraStaysForASec", CutEvent.EventType.changeCamera, false, new Vector3(-0.44f, -1.12f, -1.68f), new Vector3(0f, 0f, 0f), .02f)); //Camera stays in position
        newCut.cutsceneEvents.Add(new CutEvent("CameraFadeOut", CutEvent.EventType.fadeOut, false, 0.75f));
        newCut.cutsceneEvents.Add(new CutEvent("CameraReturns", CutEvent.EventType.changeCamera, false, new Vector3(-5.6f, 3.31f, -8f), new Vector3(0f, 0f, 0f), 0f)); //Camera returns to the child
        newCut.cutsceneEvents.Add(new CutEvent("CameraFadeOut", CutEvent.EventType.fadeIn, false, 0.75f));
        //Cutscene ends, game begins.

        //Add Cutscene to the Cutscene List
        cutscenes.Add(newCut.cutsceneName, newCut);

        #endregion

        /* //Sample Cutscene
         #region Introduction / Sample Cutscene

         //Sample Cutscene
         //Set Name
         newCut.cutsceneName = ("Sample");

         //Start Adding Sequences!
         //newCut.cutsceneEvents.Add(new CutEvent("PlaceChild", CutEvent.EventType.moveChild, true, new Vector3(10f, -0.5f, -3.58f), new Vector3(0f, 0f, 0f), 0f)); //Place starting point for Child
         //newCut.cutsceneEvents.Add(new CutEvent("PlaceGolem", CutEvent.EventType.moveGolem, true, new Vector3(-6.55f, 2.15f, 0f), new Vector3(0f, 0f, 0f), 0f)); //Place Starting Point for Golem
         newCut.cutsceneEvents.Add(new CutEvent("CameraFadeIn", CutEvent.EventType.fadeIn, false, .25f));
         newCut.cutsceneEvents.Add(new CutEvent("ChildWalksOnScreen", CutEvent.EventType.moveChild, false, new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, 0f), 1.25f)); //Child walks towards the Golem
         newCut.cutsceneEvents.Add(new CutEvent("ChildWalksOnScreen", CutEvent.EventType.moveChild, false, new Vector3(0f, 0f, 0f), new Vector3(0f, -15f, 0f), 0.5f)); //Child stops
         newCut.cutsceneEvents.Add(new CutEvent("CameraPansUp", CutEvent.EventType.changeCamera, false, new Vector3(4.95f, 0f, -2.65f), new Vector3(0f, 0f, 0f), 1f)); //Camera pans to the Golem
         newCut.cutsceneEvents.Add(new CutEvent("CameraStaysForASec", CutEvent.EventType.changeCamera, false, new Vector3(4.95f, 0f, -2.65f), new Vector3(0f, 0f, 0f), .02f)); //Camera stays in position
         newCut.cutsceneEvents.Add(new CutEvent("CameraPansUp", CutEvent.EventType.changeCamera, false, new Vector3(9.19f, -1.12f, -1.68f), new Vector3(0f, 0f, 0f), 1f)); //Camera pans to the Button
         newCut.cutsceneEvents.Add(new CutEvent("CameraStaysForASec", CutEvent.EventType.changeCamera, false, new Vector3(-0.44f, -1.12f, -1.68f), new Vector3(0f, 0f, 0f), .02f)); //Camera stays in position
         newCut.cutsceneEvents.Add(new CutEvent("CameraFadeOut", CutEvent.EventType.fadeOut, false, 0.75f));
         newCut.cutsceneEvents.Add(new CutEvent("CameraReturns", CutEvent.EventType.changeCamera, false, new Vector3(-5.6f, 3.31f, -8f), new Vector3(0f, 0f, 0f), 0f)); //Camera returns to the child
         newCut.cutsceneEvents.Add(new CutEvent("CameraFadeOut", CutEvent.EventType.fadeIn, false, 0.75f));
         //Cutscene ends, game begins.

         //Add Cutscene to the Cutscene List
         cutscenes.Add(newCut.cutsceneName, newCut);

         #endregion*/
    }


    public void AddNewCutScene(string name)
    {
        Cutscene newCut = new Cutscene(name);

        cutscenes.Add(newCut.cutsceneName, newCut);
    }

    public void AddCutSceneEvent(string cutsceneName, string eventName, CutEvent.EventType type, Vector3 position, Vector3 rotation, float length)
    {

        Cutscene targetCutscene = cutscenes[cutsceneName];

        CutEvent newEvent = new CutEvent(eventName, type, false, position, rotation, length);

        targetCutscene.cutsceneEvents.Add(newEvent);
    }
}
