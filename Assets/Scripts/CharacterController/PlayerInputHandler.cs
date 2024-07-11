using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset _playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string _groundMovementReference = "Ground";
    [SerializeField] private string _flyingMovementReference = "Flying";
    [SerializeField] private string _uiReference = "UI";

    [Header("Action Name References")]
    [SerializeField] private string _movement = "Movement";
    [SerializeField] private string _look = "Look";
    [SerializeField] private string _jump = "Jump";

    private InputAction _groundMovementAction;
    private InputAction _groundLookAction;
    private InputAction _groundjumpAction;
    private InputAction _flyingMovementAction;
    private InputAction _flyingLookAction;
    private InputAction _uiMovementAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        _groundMovementAction = _playerControls.FindActionMap(_groundMovementReference).FindAction(_movement);
        _groundLookAction = _playerControls.FindActionMap(_groundMovementReference).FindAction(_look);
        _groundjumpAction = _playerControls.FindActionMap(_groundMovementReference).FindAction(_jump);
        _flyingMovementAction = _playerControls.FindActionMap(_flyingMovementReference).FindAction(_movement);
        _flyingLookAction = _playerControls.FindActionMap(_flyingMovementReference).FindAction(_look);
        _uiMovementAction = _playerControls.FindActionMap(_uiReference).FindAction(_movement);

        _groundMovementAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _groundMovementAction.canceled += context => MoveInput = Vector2.zero;

        _groundLookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _groundLookAction.canceled += context => LookInput = Vector2.zero;

        _groundjumpAction.performed += context => JumpTriggered = true;
        _groundjumpAction.canceled += context => JumpTriggered = false;

        _flyingMovementAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _flyingMovementAction.canceled += context => MoveInput = Vector2.zero;

        _flyingLookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _flyingLookAction.canceled += context => LookInput = Vector2.zero;

        _uiMovementAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _uiMovementAction.canceled += context => MoveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        _groundMovementAction.Enable();
        _groundLookAction.Enable();
        _groundjumpAction.Enable();
        _flyingMovementAction.Enable();
        _flyingLookAction.Enable();
        _uiMovementAction.Enable();
    }

    private void OnDisable()
    {
        _groundMovementAction.Disable();
        _groundLookAction.Disable();
        _groundjumpAction.Disable();
        _flyingMovementAction.Disable();
        _flyingLookAction.Disable();
        _uiMovementAction.Disable();
    }
}
