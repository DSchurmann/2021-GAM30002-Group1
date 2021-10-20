using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject crystal;
    [SerializeField] private Material initialMaterial;
    [SerializeField] private Material savedMaterial;

    public bool Activated { get; set;}
    public bool GameSaved{ get; set;}

    // Start is called before the first frame update
    void Start()
    {
        GameSaved = false;
        crystal.GetComponent<MeshRenderer>().material = initialMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveGame()
    {
        GameController.GH.SaveGame();
        particleSystem.Play();
        crystal.GetComponent<MeshRenderer>().material = savedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ChildControllerRB>()!=null)
        {
            // save at the checkpoint
            if(!GameSaved)
            {
                SaveGame();
                GameSaved = true;
            }
            // destroy checkpoint object after save
            //if(GameSaved)
            //{
            //    Destroy(this.gameObject);
            //}
        }
    }
}
