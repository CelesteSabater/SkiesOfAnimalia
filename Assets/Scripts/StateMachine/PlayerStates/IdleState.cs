using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerController entity, Animator animator) : base(entity, animator) { }

    public override void OnEnter()
    {
        //animaciones idle
        Debug.Log("Idle.OnEnter");
    }

    public override void FixedUpdate()
    {
        entity.movement.GroundMovement(Vector2.zero, false);
        entity.movement.Jump(PlayerInputHandler.Instance.JumpTriggered, entity.GetCoyoteTime(), entity.GetJumpBuffer());
        entity.LookAt(Vector2.zero);

        if (!PlayerInputHandler.Instance.JumpTriggered) entity.physics.Float();
        entity.physics.UpdateUprigthForce();
    }
}