using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerController entity, Animator animator) : base(entity, animator) { }

    public override void OnEnter()
    {
        //animación saltar
    }

    public override void FixedUpdate()
    {
        entity.movement.GroundMovement(PlayerInputHandler.Instance.MoveInput, true);
        entity.LookAt(PlayerInputHandler.Instance.MoveInput);
        entity.physics.Fall(PlayerInputHandler.Instance.JumpTriggered);
    }
}
