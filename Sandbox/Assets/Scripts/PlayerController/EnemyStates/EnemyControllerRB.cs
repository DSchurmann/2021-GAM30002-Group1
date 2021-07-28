using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerRB : PlayerControllerRB
{
    public string _currentState;
    public NavMeshAgent nav;

    public Transform target;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        ControllerEnabled = false;

        nav = GetComponent<NavMeshAgent>();
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
