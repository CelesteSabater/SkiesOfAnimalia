using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysics : MonoBehaviour
{
    [SerializeField] private float _rayLenght = 5f, _rideHeight = 0.5f, _groundedHeight = 1f;
    [SerializeField] private float _gravity = 5f;
    [SerializeField] private float _terminalVelocity = 20f;
    [SerializeField] private float _jumpEndEarlyGravityModifier = 2f;

    private Transform _pos;
    private Vector3 _currentGround;
    private Rigidbody _rb;
    private PlayerController _controller;
    private SpringToTarget3D _springToTarget;
    private SpringToRotation _springToRotation;
    private SpringToScale _springToScale;

    private void Awake()
    {
        _currentGround = Vector3.down;
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<PlayerController>();
        _springToTarget = GetComponent<SpringToTarget3D>();
        _springToScale = GetComponent<SpringToScale>();
        _springToRotation = GetComponent<SpringToRotation>();
        _pos = GetComponent<Transform>();
    }

    public void Fall(bool jump)
    {
        float fallSpeed;
        if (jump != true && _rb.velocity.y > 0) fallSpeed = _gravity;
        else 
        {
            fallSpeed = _gravity * _jumpEndEarlyGravityModifier;
            if (_controller != null) _controller.SetCoyoteTimeZero();
        }

        Vector3 targetVel = -transform.up * fallSpeed;
        Vector3 velocityChange = targetVel - _rb.velocity;
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void Float()
    {
        RaycastHit hit = new RaycastHit();

        if (RayHitsGround(out hit))
        {
            Vector3 vel = _rb.velocity;
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;
            if (hit.rigidbody != null) { otherVel = hitBody.velocity; }

            float rayDirVel = Vector3.Dot(_currentGround, vel);
            float otherDirVel = Vector3.Dot(_currentGround, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float x = hit.distance - _rideHeight;
            float springForce = (x * _springToTarget.getStiffness()) - (relVel * _springToTarget.getDamping());
            _rb.AddForce(_currentGround * springForce);

            if (hit.rigidbody != null) { hitBody.AddForceAtPosition(_currentGround * -springForce, hit.point); }

            Debug.DrawRay(_pos.position, _rayLenght * _currentGround, Color.green);
        }
        else
        {
            Debug.DrawRay(_pos.position, _rayLenght * _currentGround, Color.red);
        }
    }

    public void UpdateUprigthForce()
    {
        var springTorque = _springToRotation.getStiffness() * Vector3.Cross(_rb.transform.up, _currentGround * -1);
        var dampTorque = _springToRotation.getDamping() * -_rb.angularVelocity;
        _rb.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
    }

    bool RayHitsGround()
    {
        return Physics.Raycast(_pos.position, _currentGround, _rayLenght);
    }

    bool RayHitsGround(out RaycastHit hit)
    {
        return Physics.Raycast(_pos.position, _currentGround, out hit, _rayLenght);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(_pos.position, _currentGround, _groundedHeight);
    }
}
