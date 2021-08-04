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


    public virtual void TakeDamage(float value)
    {
        GetComponent<StatManager>().GetStatType(StatType.health).MinusValue(value);
    }

    public void KnockBack(Transform hitObj)
    {
        var magnitude = 1;
        var force = transform.position - target.transform.position;
        force.Normalize();
        GetComponent<Rigidbody>().AddForce(force * magnitude, ForceMode.Impulse);
    }

    public virtual void HandleHit(Collider other)
    {
        // hit by player weapon
        if(other.gameObject.GetComponent<PlayerWeapon>()!= null)
        {
            Debug.Log("hit by player weapon");
            nav.isStopped = true;
            float hitValue = other.gameObject.GetComponent<PlayerWeapon>().attackDamage;
            TakeDamage(hitValue);
            //ChangeState(ish)
            //KnockBack(other.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }
}
