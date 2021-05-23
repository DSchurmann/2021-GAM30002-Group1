using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController LC;
    public bool LevelComplete { get; set; }

    public LevelCondition[] _conditions;

    //Set Singleton
    private void Awake()
    {
        //Set Singleton
        if (LC != null && LC != this)
            Destroy(this);
        else if (LC == null)
            LC = (this);

        //Don't Destroy
        DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    public void Start()
    {
        _conditions = GetComponents<LevelCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        LevelComplete = AllConditionsMet();

        if(LevelComplete)
        {
            Debug.Log("LEVEL COMPLETE");
        }
    }

    public LevelCondition[] Conditions
    {
        get { return _conditions; }
        set { _conditions = value; }
    }


    public bool AllConditionsMet()
    {
        int length = _conditions.Length;
        int count = 0;

        foreach (var item in _conditions)
        {
            if(item.ConditionMet())
            {
                count++;
            }
        }
        if (count == length)
            return true;
        return false;
    }
}
