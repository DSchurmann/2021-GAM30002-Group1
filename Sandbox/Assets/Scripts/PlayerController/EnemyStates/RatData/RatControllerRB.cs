using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatControllerRB : EnemyControllerRB
{

    // define all the states
    public RatWanderState RatWanderState { get; private set; }
    public RatAttackCoolDownState RatAttackCoolDownState { get; private set; }
    public RatAttackState RatAttackState { get; private set; }
    public RatIdleState RatIdleState { get; private set; }
    public RatChargeState RatChargeState { get; private set; }

    public override void Awake()
    {
        base.Awake();

        RatWanderState = new RatWanderState(this, "Wander");
        RatAttackCoolDownState = new RatAttackCoolDownState(this, "CoolDown");
        RatAttackState = new RatAttackState(this, "Attack");
        RatIdleState = new RatIdleState(this, "Idle");
        RatChargeState = new RatChargeState(this, "Charge");
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // set the initial state
        InitialState(RatIdleState);
    }

    // Update and FixedUpdate function
    #region Update Functions
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
       

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion

    public override void Flip()
    {
        base.Flip();
        //flip sprite
        transform.Rotate(0.0f, 180.0f, 0, 0f);
    }
}
