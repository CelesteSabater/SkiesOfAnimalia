using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : IState
{
    protected readonly PlayerController entity;
    protected readonly Animator animator;

    protected PlayerBaseState(PlayerController entity, Animator animator)
    {
        this.entity = entity;
        this.animator = animator;
    }

    public virtual void Update()
    {
        //noop
    }

    public virtual void FixedUpdate()
    {
        //noop
    }

    public virtual void OnEnter()
    {
        Debug.Log(this + ".OnEnter Default");
    }

    public virtual void OnExit()
    {
        Debug.Log(this + ".Exit Default");
    }
}
