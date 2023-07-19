using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private PlayerInput _playerInput;
    
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
    
    // Constants
    private readonly int _zero = 0;
    
    public Vector2 CurrentMovementInput { get => _currentMovementInput; set => _currentMovementInput = value; }
    public bool IsMovementPressed { get => _isMovementPressed; set => _isMovementPressed = value; }
    public bool IsActionPressed { get => _isActionPressed; }
    public bool IsActionHeld { get => _isActionHeld; }
    public bool IsLightAttackPressed { get => _isLightAttackPressed; }
    public bool IsMediumAttackPressed { get => _isMediumAttackPressed; }
    public bool IsHeavyAttackPressed { get => _isHeavyAttackPressed; }
    public bool IsBlockPressed { get => _isBlockPressed; }
    public bool IsBlockHeld { get => _isBlockHeld; }

    public void DisablePlayerInput() {
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
    public void EnablePlayerInput() {
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

    private void Awake() {
        GetComponent<GameManager>().InputSystem = this;
        _playerInput = new PlayerInput();
    }

    private void Update() {
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
    /// When block is canceled
    /// </summary>
    /// <param name="context">Reference to our movement system</param>
    void OnBlockCanceled(InputAction.CallbackContext context) {
        _isBlockPressed = false;
    }
}
