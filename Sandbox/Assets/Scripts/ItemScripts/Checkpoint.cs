using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool Activated { get; set;}
    public bool GameSaved{ get; set;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveGame()
    {
        GameController.GH.SaveGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerControllerRB>()!=null)
        {
            // save at the checkpoint
            if(!GameSaved)
            {
                SaveGame();
                GameSaved = true;
            }
            // destroy checkpoint object after save
            if(GameSaved)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
