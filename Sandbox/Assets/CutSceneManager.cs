using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CutSceneManager : MonoBehaviour
{
    public Director Director;
    public List<Cutscene> cutscenes = new List<Cutscene>();


    [Header("Data")]
    public int NumberOfCutscenes;

    private void OnEnable()
    {
        PopulateManager();
        //NumberOfCutscenes = cutscenes.Count;
        //PopulateDirector();
        //NumberOfCutscenes_Director = Director.cutscenes.Count;
    }

    void Awake()
    {
        if(Director == null)
        {
            Director = GameController.GH.GetComponent<Director>();
        }

        PopulateDirector();
    }

    // Start is called before the first frame update
    void Start()
    {
        //PopulateManager();
        //PopulateDirector();
        //Director.StartCutscene(Director.cutscenes["Intro"]);
        //NumberOfCutscenes = cutscenes.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ppulate list with cut scene objects
    public void PopulateManager()
    {
       /* foreach (Cutscene item in transform.GetComponentsInChildren<Cutscene>())
        {
            cutscenes.Add(item);
        }*/

        NumberOfCutscenes = cutscenes.Count;

    }

    // send cutscenes to the Director object
    public void PopulateDirector()
    {
        if (cutscenes.Count > 0)
        {
            Dictionary<string, Cutscene> cutscenes_dict = new Dictionary<string, Cutscene>();

            foreach (var item in cutscenes)
            {
                cutscenes_dict.Add(item.cutsceneName, item);
            }

            Director.cutscenes = cutscenes_dict;
        }
    }
}
