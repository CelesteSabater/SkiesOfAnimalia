using UnityEngine;

public abstract class BaseState : IState
{
    protected readonly IGameObjectController entity;
    protected readonly Animator animator;

    protected BaseState(IGameObjectController entity, Animator animator)
    {
        this.entity = entity;
        this.animator = animator;
    }

    public virtual void FixedUpdate()
    {
        //noop
    }

    public virtual void OnEnter()
    {
        //noop
    }

    public virtual void OnExit()
    {
        //noop
    }

    public virtual void Update()
    {
        //noop
    }
}
