using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObjectController
{
    public CharacterMovement movement { get; set; }
    public CharacterPhysics physics { get; set; }
    public StateMachine stateMachine { get; set; }
    public Animator animator { get; set; }
}
