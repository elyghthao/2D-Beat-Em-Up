using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour {
    
    // Inspector Arguments
    [Header("Body Pieces")]
    public GameObject body;
    
    [Header("Attack Boundaries")]
    public GameObject heavyAttackBounds;
    public GameObject mediumAttackBounds;
    public GameObject lightAttackBounds;
    
    [Header("FrameData")] 
    public int framesPerSecond;
    
    [Header("HeavyAttack")]
    public int heavyFrameCount = 44;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 heavyStartupFrames;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 heavyActiveFrames;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 heavyRecoveryFrames;
    
    [Header("MediumAttack")]
    public int mediumFrameCount = 32;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 mediumStartupFrames;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 mediumActiveFrames;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 mediumRecoveryFrames;
    
    [Header("LightAttack")]
    public int lightFrameCount = 23;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 lightStartupFrames;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 lightActiveFrames;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 lightRecoveryFrames;
    
    [Header("Movement")]
    public float movementSpeed;

    // Reference variables
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Material _heavyBoundsMat;
    private Material _mediumBoundsMat;
    private Material _lightBoundsMat;

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
    public Material HeavyBoundsMat { get => _heavyBoundsMat; set => _heavyBoundsMat = value; }
    public Material MediumBoundsMat { get => _mediumBoundsMat; set => _mediumBoundsMat = value; }
    public Material LightBoundsMat { get => _lightBoundsMat; set => _lightBoundsMat = value; }
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public bool IsActionPressed { get => _isActionPressed; set => _isActionPressed = value; }
    public bool IsLightAttackPressed { get => _isLightAttackPressed; set => _isLightAttackPressed = value; }
    public bool IsMediumAttackPressed { get => _isMediumAttackPressed; set => _isMediumAttackPressed = value; }
    public bool IsHeavyAttackPressed { get => _isHeavyAttackPressed; set => _isHeavyAttackPressed = value; }
    public bool IsBlockPressed { get => _isBlockPressed; set => _isBlockPressed = value; }
    
    // Functions
    private void Awake() {
        
        // DELETE THIS ONCE PROPER SETTINGS ARE IN PLACE
        Application.targetFrameRate = 60;
        
        _playerInput = new PlayerInput();
        _states = new PlayerStateFactory(this);
        _baseMaterial = body.GetComponent<Renderer>().material;
        _heavyBoundsMat = heavyAttackBounds.GetComponent<Renderer>().material;
        _mediumBoundsMat = mediumAttackBounds.GetComponent<Renderer>().material;
        _lightBoundsMat = lightAttackBounds.GetComponent<Renderer>().material;

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
        
        _playerInput.Player.LightAttack.performed += OnLightAttackPerformed;
        _playerInput.Player.LightAttack.canceled += OnLightAttackCanceled;
        
        _playerInput.Player.MediumAttack.performed += OnMediumAttackPerformed;
        _playerInput.Player.MediumAttack.canceled += OnMediumAttackCanceled;
        
        _playerInput.Player.HeavyAttack.performed += OnHeavyAttackPerformed;
        _playerInput.Player.HeavyAttack.canceled += OnHeavyAttackCanceled;
        
        _playerInput.Player.Block.performed += OnBlockPerformed;
        _playerInput.Player.Block.canceled += OnBlockCanceled;
    }

    private void OnDisable() {
        _playerInput.Disable();
        _playerInput.Player.Movement.performed -= OnMovementPerformed;
        _playerInput.Player.Movement.canceled -= OnMovementCanceled;
        
        _playerInput.Player.LightAttack.performed -= OnLightAttackPerformed;
        _playerInput.Player.LightAttack.canceled -= OnLightAttackCanceled;
        
        _playerInput.Player.MediumAttack.performed -= OnMediumAttackPerformed;
        _playerInput.Player.MediumAttack.canceled -= OnMediumAttackCanceled;
        
        _playerInput.Player.HeavyAttack.performed -= OnHeavyAttackPerformed;
        _playerInput.Player.HeavyAttack.canceled -= OnHeavyAttackCanceled;
        
        _playerInput.Player.Block.performed -= OnBlockPerformed;
        _playerInput.Player.Block.canceled -= OnBlockCanceled;
    }

    // Update is called once per frame
    void Update() {
        _currentState.UpdateState();
        CheckActionPressed();
    }

    void CheckActionPressed() {
        PlayerInput.PlayerActions pAction = _playerInput.Player;
        _isActionPressed = pAction.LightAttack.WasPerformedThisFrame() ||
                           pAction.MediumAttack.WasPerformedThisFrame() ||
                           pAction.HeavyAttack.WasPerformedThisFrame();
    }

    void OnMovementPerformed(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }
    
    void OnMovementCanceled(InputAction.CallbackContext context) {
        _currentMovementInput = Vector2.zero;
        _isMovementPressed = false;
    }

    void OnLightAttackPerformed(InputAction.CallbackContext context) {
        _isLightAttackPressed = context.ReadValueAsButton();
    }
    void OnLightAttackCanceled(InputAction.CallbackContext context) {
        _isLightAttackPressed = context.ReadValueAsButton();
    }
    
    void OnMediumAttackPerformed(InputAction.CallbackContext context) {
        _isMediumAttackPressed = context.ReadValueAsButton();
    }
    void OnMediumAttackCanceled(InputAction.CallbackContext context) {
        _isMediumAttackPressed = context.ReadValueAsButton();
    }
    
    void OnHeavyAttackPerformed(InputAction.CallbackContext context) {
        _isHeavyAttackPressed = context.ReadValueAsButton();
    }
    void OnHeavyAttackCanceled(InputAction.CallbackContext context) {
        _isHeavyAttackPressed = context.ReadValueAsButton();
    }
    
    void OnBlockPerformed(InputAction.CallbackContext context) {
        _isBlockPressed = context.ReadValueAsButton();
    }
    void OnBlockCanceled(InputAction.CallbackContext context) {
        _isBlockPressed = context.ReadValueAsButton();
    }

    public void SpeedControl() {
        Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
      
        // limit velocity if needed
        if (flatVelocity.magnitude > movementSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z);
        }
    }
}
