using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Parameterss
    public static GameController GH;
    

    public PlayerControllerRB childObj;
    public PlayerControllerRB golemObj;
    public enum CurrentPlayer { Child, Golem }
    public CurrentPlayer currentPlayer;

    
    // Set Singleton
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

    void SwapPlayer()
    {
        if(currentPlayer == CurrentPlayer.Child)
        {
            currentPlayer = CurrentPlayer.Golem;
        }
        else if (currentPlayer == CurrentPlayer.Golem)
        {
            currentPlayer = CurrentPlayer.Child;
        }
        Debug.Log("Character Swapped!");
    }
}
