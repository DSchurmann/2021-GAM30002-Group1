using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected CharacterController cc;

    [SerializeField] protected float maxMoveSpeed;

    protected bool canControl;

    private void Update()
    {
        if (canControl)
        {
            float moveSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * maxMoveSpeed;

            cc.Move(new Vector3(moveSpeed, 0, 0));
        }
    }

    //add action methods for child and golem use - 1 for each face button
    protected abstract void Ability1();
    protected abstract void Ability2();
    protected abstract void Ability3();
    protected abstract void Ability4();

    public void CanControl(bool input)
    {
        canControl = input;
    }
}