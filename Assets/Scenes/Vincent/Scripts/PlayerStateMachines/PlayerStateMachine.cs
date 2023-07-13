using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Context file that holds important information for all player states to reference
/// </summary>
public class PlayerStateMachine : MonoBehaviour {
    
    // Inspector Arguments
    [Header("Body Elements")]
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
    public Vector2 heavyStartupFrames = new Vector2(1, 10);

    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 heavyActiveFrames = new Vector2(11, 15);
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 heavyRecoveryFrames = new Vector2(16, 44);
    
    [Header("MediumAttack")]
    public int mediumFrameCount = 32;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 mediumStartupFrames = new Vector2(1, 9);
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 mediumActiveFrames = new Vector2(10, 14);
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 mediumRecoveryFrames = new Vector2(15, 32);
    
    [Header("LightAttack")]
    public int lightFrameCount = 23;
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 lightStartupFrames = new Vector2(1, 7);
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 lightActiveFrames = new Vector2(8, 9);
    [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
    public Vector2 lightRecoveryFrames = new Vector2(10, 23);
    
    [Header("Movement")]
    public float movementSpeed;

    // Reference variables
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Material _heavyBoundsMat;
    private Material _mediumBoundsMat;
    private Material _lightBoundsMat;
    private Material _baseMaterial;

    // State variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    
    // Input values
    private Vector2 _currentMovementInput;
    private bool _isMovementPressed;
    private bool _isActionPressed;
    private bool _isActionHeld;
    private bool _isLightAttackPressed;
    private bool _isMediumAttackPressed;
    private bool _isHeavyAttackPressed;
    private bool _isBlockPressed;
    private bool _isBlockHeld;
    
    // Other Variables
    private bool _characterFlipped;

    // Constants
    private readonly int _zero = 0;

    // Getters and Setters
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Vector2 CurrentMovementInput { get => _currentMovementInput; set => _currentMovementInput = value; }
    public bool IsMovementPressed { get => _isMovementPressed; set => _isMovementPressed = value; }
    public Material BaseMaterial { get => _baseMaterial; set => _baseMaterial = value; }
    public Material HeavyBoundsMat { get => _heavyBoundsMat; set => _heavyBoundsMat = value; }
    public Material MediumBoundsMat { get => _mediumBoundsMat; set => _mediumBoundsMat = value; }
    public Material LightBoundsMat { get => _lightBoundsMat; set => _lightBoundsMat = value; }
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public bool IsActionPressed { get => _isActionPressed; }
    public bool IsActionHeld { get => _isActionHeld; }
    public bool IsLightAttackPressed { get => _isLightAttackPressed; }
    public bool IsMediumAttackPressed { get => _isMediumAttackPressed; }
    public bool IsHeavyAttackPressed { get => _isHeavyAttackPressed; }
    public bool IsBlockPressed { get => _isBlockPressed; }
    public bool IsBlockHeld { get => _isBlockHeld; }
    public bool CharacterFlipped { get => _characterFlipped; set => _characterFlipped = value; }

    // Functions
    private void Awake() {
                
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

    /// <summary>
    /// Enables all input for the character when the PlayerStateMachine script is enabled
    /// </summary>
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

    /// <summary>
    /// Disables all input for the character when the PlayerStateMachine script is disabled
    /// </summary>
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
        _currentState.UpdateStates();
        CheckActionPressed();
    }

    /// <summary>
    /// Checks for when certain buttons are pressed, is updated constantly so we can be sure any held, or immediately
    /// pressed buttons are accurate
    /// </summary>
    void CheckActionPressed() {
        PlayerInput.PlayerActions pAction = _playerInput.Player;
        // Defies if an action was pressed, not held
        _isActionPressed = pAction.LightAttack.WasPerformedThisFrame() ||
                           pAction.MediumAttack.WasPerformedThisFrame() ||
                           pAction.HeavyAttack.WasPerformedThisFrame() || pAction.Block.WasPerformedThisFrame();
        // Defines if an action is being held down
        _isActionHeld = pAction.LightAttack.IsPressed() || pAction.MediumAttack.IsPressed() ||
                        pAction.HeavyAttack.IsPressed() || pAction.Block.IsPressed();
        // Defines if the block button is currently being held down
        _isBlockHeld = pAction.Block.IsPressed();
    }

    /// <summary>
    /// When a movement action is performed
    /// </summary>
    /// <param name="context">Reference to our input system</param>
    void OnMovementPerformed(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }
    
    /// <summary>
    /// When a movement action is canceled
    /// </summary>
    /// <param name="context">Reference to our input system</param>
    void OnMovementCanceled(InputAction.CallbackContext context) {
        _currentMovementInput = Vector2.zero;
        _isMovementPressed = false;
    }

    /// <summary>
    /// When a light attack is performed
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnLightAttackPerformed(InputAction.CallbackContext context) {
        _isLightAttackPressed = context.ReadValueAsButton();
    }
    /// <summary>
    /// When a light attack is canceled
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnLightAttackCanceled(InputAction.CallbackContext context) {
        _isLightAttackPressed = false;
    }
    /// <summary>
    /// When a medium attack is performed
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnMediumAttackPerformed(InputAction.CallbackContext context) {
        _isMediumAttackPressed = context.ReadValueAsButton();
    }
    /// <summary>
    /// When a medium attack is canceled
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnMediumAttackCanceled(InputAction.CallbackContext context) {
        _isMediumAttackPressed = false;
    }
    /// <summary>
    /// When a heavy attack is performed
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnHeavyAttackPerformed(InputAction.CallbackContext context) {
        _isHeavyAttackPressed = context.ReadValueAsButton();
    }
    /// <summary>
    /// When a heavy attack is canceled
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnHeavyAttackCanceled(InputAction.CallbackContext context) {
        _isHeavyAttackPressed = false;
    }
    /// <summary>
    /// When block is performed
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnBlockPerformed(InputAction.CallbackContext context) {
        _isBlockPressed = context.ReadValueAsButton();
    }
    /// <summary>
    /// When block is cancelee
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnBlockCanceled(InputAction.CallbackContext context) {
        _isBlockPressed = false;
    }

    /// <summary>
    /// Calculates the speed of our character
    /// </summary>
    public void SpeedControl() {
        Vector3 playerVelocity = Rigidbody.velocity;
        Vector3 flatVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);
      
        // limit velocity if needed
        if (flatVelocity.magnitude > movementSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z);
        }
    }

    /// <summary>
    /// Flips our character object to face the other direction
    /// </summary>
    public void FlipCharacter() {
        _characterFlipped = !_characterFlipped;
        Debug.Log("Character flipped: " + _characterFlipped);
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }
}
