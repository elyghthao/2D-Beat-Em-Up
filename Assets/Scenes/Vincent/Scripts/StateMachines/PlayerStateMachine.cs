using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour {
    
    // Inspector Arguments
    public GameObject body;
    [Header("Movement")]
    public float movementSpeed;
    public Transform orientation;

    // Reference variables
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    // Components
    private Material _baseMaterial;

    // State variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    
    // Input values
    private Vector2 _currentMovementInput;
    private bool _isMovementPressed;
    private bool _isActionPressed;
    private bool _isLightAttackPressed;
    private bool _isMediumAttackPressed;
    private bool _isHeavyAttackPressed;
    private bool _isBlockPressed;
    

    // Constants
    private int _zero = 0;

    // Getters and Setters
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Vector2 CurrentMovementInput { get => _currentMovementInput; set => _currentMovementInput = value; }
    public bool IsMovementPressed { get => _isMovementPressed; set => _isMovementPressed = value; }
    public Material BaseMaterial { get => _baseMaterial; set => _baseMaterial = value; }
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public bool IsActionPressed { get => _isActionPressed; set => _isActionPressed = value; }
    public bool IsLightAttackPressed { get => _isLightAttackPressed; set => _isLightAttackPressed = value; }
    public bool IsMediumAttackPressed { get => _isMediumAttackPressed; set => _isMediumAttackPressed = value; }
    public bool IsHeavyAttackPressed { get => _isHeavyAttackPressed; set => _isHeavyAttackPressed = value; }
    public bool IsBlockPressed { get => _isBlockPressed; set => _isBlockPressed = value; }
    
    // Functions
    private void Awake() {
        _playerInput = new PlayerInput();
        _states = new PlayerStateFactory(this);
        _baseMaterial = body.GetComponent<Renderer>().material;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        
        // enter initial state. All assignments should go before here
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    private void OnEnable() {
        _playerInput.Enable();
        _playerInput.Player.Movement.performed += OnMovementPerformed;
        _playerInput.Player.Movement.canceled += OnMovementCanceled;
        _playerInput.Player.LightAttack.canceled += OnActionCanceled;
        _playerInput.Player.MediumAttack.performed += OnActionPerformed;
        _playerInput.Player.MediumAttack.canceled += OnActionCanceled;
        _playerInput.Player.HeavyAttack.performed += OnActionPerformed;
        _playerInput.Player.HeavyAttack.canceled += OnActionCanceled;
        _playerInput.Player.Block.performed += OnActionPerformed;
        _playerInput.Player.Block.canceled += OnActionCanceled;
    }

    private void OnDisable() {
        _playerInput.Disable();
        _playerInput.Player.Movement.performed -= OnMovementPerformed;
        _playerInput.Player.Movement.canceled -= OnMovementCanceled;
        _playerInput.Player.LightAttack.performed -= OnActionPerformed;
        _playerInput.Player.LightAttack.canceled -= OnActionCanceled;
        _playerInput.Player.MediumAttack.performed -= OnActionPerformed;
        _playerInput.Player.MediumAttack.canceled -= OnActionCanceled;
        _playerInput.Player.HeavyAttack.performed -= OnActionPerformed;
        _playerInput.Player.HeavyAttack.canceled -= OnActionCanceled;
        _playerInput.Player.Block.performed -= OnActionPerformed;
        _playerInput.Player.Block.canceled -= OnActionCanceled;
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();
    }

    void OnMovementPerformed(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }
    
    void OnMovementCanceled(InputAction.CallbackContext context) {
        _currentMovementInput = Vector2.zero;
        _isMovementPressed = false;
    }

    void OnActionPerformed(InputAction.CallbackContext context) {
        if (context.action == _playerInput.Player.LightAttack) {
            _isLightAttackPressed = true;
        } else if (context.action == _playerInput.Player.MediumAttack) {
            _isMediumAttackPressed = true;
        } else if (context.action == _playerInput.Player.HeavyAttack) {
            _isHeavyAttackPressed = true;
        } else if (context.action == _playerInput.Player.Block) {
            _isBlockPressed = true;
        }
    }
    void OnActionCanceled(InputAction.CallbackContext context) {
        if (context.action == _playerInput.Player.LightAttack) {
            _isLightAttackPressed = false;
        } else if (context.action == _playerInput.Player.MediumAttack) {
            _isMediumAttackPressed = false;
        } else if (context.action == _playerInput.Player.HeavyAttack) {
            _isHeavyAttackPressed = false;
        } else if (context.action == _playerInput.Player.Block) {
            _isBlockPressed = false;
        }
    }
}
