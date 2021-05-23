using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCondition : MonoBehaviour
{
    public string _name;
    public bool _value;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool ConditionMet()
    {
        return _value;
    }

}
