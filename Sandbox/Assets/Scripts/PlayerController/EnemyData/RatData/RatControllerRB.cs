using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatControllerRB : StateMachine
{

    // define all the states
    public RatWanderState RatWanderState { get; private set; }
    public RatAttackCoolDownState RatAttackCoolDownState { get; private set; }
    public RatAttackState RatAttackState { get; private set; }
    public RatIdleState RatIdleState { get; private set; }
    public RatChargeState RatChargeState { get; private set; }

    void Awake()
    {
        RatWanderState = new RatWanderState();
        RatAttackCoolDownState = new RatAttackCoolDownState();
        RatAttackState = new RatAttackState();
        RatIdleState = new RatIdleState();
        RatChargeState = new RatChargeState();
    }

    // Start is called before the first frame update
    void Start()
    {
        // set the initial state
        InitialState(RatIdleState);
    }

    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public virtual void Update()
    {
        // update current state
        if (CurrentState != null)
            CurrentState.Update();
    }

    public virtual void FixedUpdate()
    {
        // fixed update current state
        if (CurrentState != null)
            CurrentState.FixedUpdate();
    }
    #endregion
}
