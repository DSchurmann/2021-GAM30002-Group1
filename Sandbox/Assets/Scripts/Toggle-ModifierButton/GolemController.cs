using System;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : Controller
{
    [SerializeField] Transform child;
    private Vector3 targetPos;
    private bool toggleState = true;

    private void Update()
    {
        if (!toggleState && !canControl)
        {
            targetPos = new Vector3(child.position.x, transform.position.y, transform.position.z);
            Vector3 offset = targetPos - transform.position;

            if(offset.magnitude > 0.5f)
            {
                offset = offset.normalized * maxMoveSpeed;
                cc.Move(offset * Time.deltaTime);
            }
        }

        if (canControl)
        {
            float moveSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * maxMoveSpeed;

            cc.Move(new Vector3(moveSpeed, 0, 0));
        }
    }

    protected override void Ability1() { throw new NotImplementedException(); }
    protected override void Ability2() { throw new NotImplementedException(); }
    protected override void Ability3() { throw new NotImplementedException(); }
    protected override void Ability4() { throw new NotImplementedException(); }

    public void SetToggleState(bool input)
    {
        toggleState = input;
    }

}
