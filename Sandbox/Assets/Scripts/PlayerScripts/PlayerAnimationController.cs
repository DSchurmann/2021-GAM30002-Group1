using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMovement();
    }

    private void AnimateMovement()
    {
        animator.SetFloat("Velocity", controller.velocity.magnitude);
    }

    public void SetCarrying(bool value)
    {
        animator.SetBool("Carrying_01", value);
    }

    public void SetJumping(bool value)
    {
        animator.SetBool("Jumping", value);
    }
}
