using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour {
    
    // Reference variables
    private PlayerInput _playerInput;
    
    // State variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    
    // Input values
    private Vector2 _currentMovementInput;
    private bool _isMovementPressed;

    // Constants
    private int _zero = 0;

    // Getters and Setters
    
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Vector2 CurrentMovementInput { get => _currentMovementInput; set => _currentMovementInput = value; }
    public bool IsMovementPressed { get => _isMovementPressed; set => _isMovementPressed = value; }
    private void Awake() {
        _playerInput = new PlayerInput();
        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    private void OnEnable() {
        _playerInput.Enable();
        _playerInput.Player.Movement.performed += OnMovementPerformed;
        _playerInput.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable() {
        _playerInput.Disable();
        _playerInput.Player.Movement.performed -= OnMovementPerformed;
        _playerInput.Player.Movement.canceled -= OnMovementCancelled;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMovementPerformed(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }
    
    void OnMovementCancelled(InputAction.CallbackContext context) {
        _currentMovementInput = Vector2.zero;
        _isMovementPressed = false;
    }

    void OnActionInput(InputAction.CallbackContext context) {
        
    }
}
