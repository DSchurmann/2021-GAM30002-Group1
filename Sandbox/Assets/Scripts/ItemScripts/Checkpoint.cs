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

    public AudioClip activateSound;

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
        CameraFollow camera = FindObjectOfType<CameraFollow>();
        if (camera) camera.Shake(3f, 0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ChildControllerRB>()!=null)
        {
            // save at the checkpoint
            if(!GameSaved)
            {
                other.gameObject.GetComponent<AudioSource>().pitch = 1f;
                other.gameObject.GetComponent<AudioSource>().PlayOneShot(activateSound);
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
