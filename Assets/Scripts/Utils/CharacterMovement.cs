using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    [SerializeField] private float _maxSpeedGround = 8f;
    [SerializeField] private float _maxAccelForceGround = 150f;

    [Header("Jumping")]
    [SerializeField] private float _jumpUpForce = 20f;
    [SerializeField] private float _jumpCooldoown = 0.5f; 
    [SerializeField] private float _airMultiplier = 0.4f;
    private bool _readyToJump = true;

    [Header("Flying")]
    [SerializeField] private float _maxSpeedFlying = 30f;
    [SerializeField] private float _maxAccelForceFlying = 300f;

    [Header("Stun")]
    [SerializeField] private float _movementControlDisabledMaxTimer = 0.5f;
    [SerializeField] private float _movementControlDisabledTimer;

    private Rigidbody _rb;
    private CharacterPhysics _physics;

    public void Awake()
    {
        _movementControlDisabledTimer = 0f;
        _rb = GetComponent<Rigidbody>();
        _physics = GetComponent<CharacterPhysics>();

        //hacer ratón invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GroundMovement(Vector2 dir, bool jumping)
    {
        if (_movementControlDisabledTimer > 0f)
        {
            dir = Vector2.zero;
            _movementControlDisabledTimer -= Time.deltaTime;
        }
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        Vector3 moveGoal = (forward * dir.y) + (right * dir.x);
        moveGoal.Normalize();
        moveGoal.y = 0;

        Vector3 targetVel = jumping == true ? moveGoal * _maxSpeedGround * _airMultiplier : moveGoal * _maxSpeedGround;

        Vector3 velocityChange = targetVel - _rb.velocity;
        Vector3.ClampMagnitude(velocityChange, _maxAccelForceGround);
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void FlyingMovement()
    {
    
    }

    public void Jump(bool jump, float coyoteTime, float jumpBuffer)
    {
        //añadir "coyote time" y bufer que haga saltar aunque aún no haya tocado el suelo
        if (jump && _readyToJump && coyoteTime > 0f)
        {
            Jump();
        }
    }

    void Jump()
    {
        _readyToJump = false;
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpUpForce, _rb.velocity.z);
        Debug.Log(_rb.velocity);

        Invoke(nameof(ResetJump), _jumpCooldoown);
    }

    void ResetJump()
    {
        _readyToJump = true;
    }
}
