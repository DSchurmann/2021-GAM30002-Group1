using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Parameters -- Core
    public static GameController GH;
    

    public bool switchMode; //Is the Golem controlled?
    public bool waitMode; //Is Wait Mode on?
    public PlayerControllerRB childObj;
    public PlayerControllerRB golemObj;


    //Parameters -- UI
    //UI Objects to be set Here -- CODE
    public GameObject waitObj;
    public List<GameObject> runeObjects = new List<GameObject>(); //Inspector
    public GameObject runeUIParent; //Inspector

    //AWAKE: Set Singleton
    private void Awake()
    {
        //Set Singleton
        if (GH != null && GH != this)
            Destroy(this);
        else if (GH == null)
            GH = (this);

        //Don't Destroy
        DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    // Get current player
    public PlayerControllerRB CurrentPlayer()
    {
        if (golemObj.ControllerEnabled) { return golemObj; }
        else
        if (childObj.ControllerEnabled) { return childObj; }
        else
        {
            Debug.LogError("NO CURRENT PLAYER");
            return null;
        }
           
    }
}
