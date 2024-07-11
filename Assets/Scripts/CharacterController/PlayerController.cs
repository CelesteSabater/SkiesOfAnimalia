using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IGameObjectController
{
    public CharacterMovement movement { get; set; }
    public CharacterPhysics physics { get; set; }
    public StateMachine stateMachine { get; set; }
    public Animator animator { get; set; }

    [Header("Movement")]
    [SerializeField] private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter = 0f;
    [SerializeField] private float _jumpBuffer = 0.2f;
    private float _jumpBufferCounter = 0f;

    [Header("Combat")]
    [SerializeField] private Transform _combatLookAt;

    [Header("Camera")]
    [SerializeField] private float _rotationSpeed = 7f;
    [SerializeField] private GameObject _basicCam;
    [SerializeField] private GameObject _combatCam;
    [SerializeField] private GameObject _flyingCam;
    private CameraStyle _currentStyle;

    private enum CameraStyle
    {
        Basic,
        Combat,
        Flying
    }

    public PlayerController(CharacterMovement movement, CharacterPhysics physics)
    {
        this.movement = movement;
        this.physics = physics;
    }

    public void Awake()
    {
        if (movement == null) { movement = GetComponent<CharacterMovement>(); }
        if (physics == null) { physics = GetComponent<CharacterPhysics>(); }
        if (animator == null) { animator = GetComponent<Animator>(); }

        //Camera
        SwitchCameraStyle(CameraStyle.Basic);

        //State Machine Config
        stateMachine = new StateMachine(name);

        //States declaration
        var groundMovementState = new GroundMovementState(this, animator);
        var jumpState = new JumpState(this, animator);
        var idleState = new IdleState(this, animator);

        //States transitions declaration
        At(jumpState, idleState, new FuncPredicate(() => physics.IsGrounded()));
        At(idleState, jumpState, new FuncPredicate(() => !physics.IsGrounded()));
        At(groundMovementState, jumpState, new FuncPredicate(() => !physics.IsGrounded()));
        At(idleState, groundMovementState, new FuncPredicate(() => PlayerInputHandler.Instance.MoveInput != Vector2.zero));
        At(groundMovementState, idleState, new FuncPredicate(() => PlayerInputHandler.Instance.MoveInput == Vector2.zero));

        //State machine initiation
        stateMachine.SetState(idleState);
    }

    public void Update() 
    {
        stateMachine.UpdateState();

        _coyoteTimeCounter = physics.IsGrounded() ? _coyoteTime : _coyoteTimeCounter - Time.deltaTime;
    }

    public void FixedUpdate()
    {
        stateMachine.FixedUpdateState();
    }

    public float GetCoyoteTime() => _coyoteTimeCounter;

    public void SetCoyoteTimeZero() => _coyoteTimeCounter = 0;

    public float GetJumpBuffer() => _jumpBufferCounter;

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    public void LookAt(Vector2 dir)
    {
        Vector3 moveGoal = Vector3.zero;
        GameObject cam = Camera.main.gameObject;
        Vector3 viewDir;
        if (_currentStyle == CameraStyle.Basic)
        {
            if (dir != Vector2.zero)
            {
                viewDir = transform.position - cam.transform.position;
                cam.transform.forward = viewDir.normalized;
                moveGoal = (cam.transform.forward * dir.y) + (cam.transform.right * dir.x);
                transform.forward = Vector3.Slerp(transform.forward, moveGoal.normalized, Time.deltaTime * _rotationSpeed);
            }
        }
        else if (_currentStyle == CameraStyle.Combat)
        {
            viewDir = _combatLookAt.position - new Vector3(cam.transform.position.x, _combatLookAt.position.y, cam.transform.position.z);
            transform.forward = Vector3.Slerp(transform.forward, viewDir.normalized, Time.deltaTime * _rotationSpeed);
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        _basicCam.SetActive(false);
        _combatCam.SetActive(false);
        _flyingCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) _basicCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) _combatCam.SetActive(true);
        if (newStyle == CameraStyle.Flying) _flyingCam.SetActive(true);

        _currentStyle = newStyle;
    }
}
