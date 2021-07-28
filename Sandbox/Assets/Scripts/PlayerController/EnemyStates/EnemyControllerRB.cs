using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerRB : PlayerControllerRB
{
    public string _currentState;


    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        ControllerEnabled = false;
    }

    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        _currentState = CurrentState.GetType().Name;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion


}
