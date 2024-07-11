using UnityEngine;

public class GroundMovementState : PlayerBaseState
{
    public GroundMovementState(PlayerController entity, Animator animator) : base(entity, animator) { }

    public override void OnEnter()
    {
        //animaciones correr
        Debug.Log("GroundMovement.OnEnter");
    }

    public override void FixedUpdate()
    {
        entity.movement.GroundMovement(PlayerInputHandler.Instance.MoveInput, false);
        entity.movement.Jump(PlayerInputHandler.Instance.JumpTriggered, entity.GetCoyoteTime(), entity.GetJumpBuffer());
        entity.LookAt(PlayerInputHandler.Instance.MoveInput);

        entity.physics.Float();
        entity.physics.UpdateUprigthForce();
    }
}