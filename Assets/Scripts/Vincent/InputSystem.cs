using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour {
   // Constants
   private readonly int _zero = 0;

   // Input values
   private Vector2 _currentMovementInput;
   private bool _enableWhenReady;
   private PlayerInput _playerInput;

   public Vector2 CurrentMovementInput {
      get => _currentMovementInput;
      set => _currentMovementInput = value;
   }

   public bool IsMovementPressed { get; set; }

   public bool IsActionPressed { get; private set; }

   public bool IsActionHeld { get; private set; }

   public bool IsLightAttackPressed { get; private set; }

   public bool IsMediumAttackPressed { get; private set; }

   public bool IsHeavyAttackPressed { get; private set; }

   public bool IsBlockPressed { get; private set; }

   public bool IsBlockHeld { get; private set; }

   public bool EmptyPlayerInput => _playerInput == null;

   public void Awake() {
      if (GameManager.Instance != null && GameManager.Instance != gameObject.GetComponent<GameManager>()) {
         Destroy(this);
         return;
      }

      _playerInput = new PlayerInput();
   }

   private void Update() {
      CheckActionPressed();
   }

   private void OnDestroy() {
      if (GameManager.Instance != null && GameManager.Instance != gameObject.GetComponent<GameManager>()) {
         Destroy(this);
         return;
      }

      Debug.LogWarning("ORIGINAL INPUT DESTROYED, THIS SHOULDN'T HAPPEN UNLESS STOPPING GAME");
   }

   public void DisablePlayerInput() {
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

   /// <summary>
   ///    Checks for when certain buttons are pressed, is updated constantly so we can be sure any held, or immediately
   ///    pressed buttons are accurate
   /// </summary>
   private void CheckActionPressed() {
      var pAction = _playerInput.Player;
      // Defies if an action was pressed, not held
      IsActionPressed = pAction.LightAttack.WasPerformedThisFrame() ||
                        pAction.MediumAttack.WasPerformedThisFrame() ||
                        pAction.HeavyAttack.WasPerformedThisFrame() || pAction.Block.WasPerformedThisFrame();
      // Defines if an action is being held down
      IsActionHeld = pAction.LightAttack.IsPressed() || pAction.MediumAttack.IsPressed() ||
                     pAction.HeavyAttack.IsPressed() || pAction.Block.IsPressed();
      // Defines if the block button is currently being held down
      IsBlockHeld = pAction.Block.IsPressed();
   }

   /// <summary>
   ///    When a movement action is performed
   /// </summary>
   /// <param name="context">Reference to our input system</param>
   private void OnMovementPerformed(InputAction.CallbackContext context) {
      _currentMovementInput = context.ReadValue<Vector2>();
      IsMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
   }

   /// <summary>
   ///    When a movement action is canceled
   /// </summary>
   /// <param name="context">Reference to our input system</param>
   private void OnMovementCanceled(InputAction.CallbackContext context) {
      _currentMovementInput = Vector2.zero;
      IsMovementPressed = false;
   }

   /// <summary>
   ///    When a light attack is performed
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnLightAttackPerformed(InputAction.CallbackContext context) {
      IsLightAttackPressed = context.ReadValueAsButton();

      // ADDED BY ABDUL: To remedy 
      if (IsLightAttackPressed) {
         IsMediumAttackPressed = false;
         IsHeavyAttackPressed = false;
         IsBlockPressed = false;
      }
   }

   /// <summary>
   ///    When a light attack is canceled
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnLightAttackCanceled(InputAction.CallbackContext context) {
      IsLightAttackPressed = false;
   }

   /// <summary>
   ///    When a medium attack is performed
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnMediumAttackPerformed(InputAction.CallbackContext context) {
      IsMediumAttackPressed = context.ReadValueAsButton();

      // ADDED BY ABDUL: To remedy 
      if (IsMediumAttackPressed) {
         IsHeavyAttackPressed = false;
         IsLightAttackPressed = false;
         IsBlockPressed = false;
      }
   }

   /// <summary>
   ///    When a medium attack is canceled
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnMediumAttackCanceled(InputAction.CallbackContext context) {
      IsMediumAttackPressed = false;
   }

   /// <summary>
   ///    When a heavy attack is performed
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnHeavyAttackPerformed(InputAction.CallbackContext context) {
      IsHeavyAttackPressed = context.ReadValueAsButton();

      // ADDED BY ABDUL: To remedy 
      if (IsHeavyAttackPressed) {
         IsMediumAttackPressed = false;
         IsLightAttackPressed = false;
         IsBlockPressed = false;
      }
   }

   /// <summary>
   ///    When a heavy attack is canceled
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnHeavyAttackCanceled(InputAction.CallbackContext context) {
      IsHeavyAttackPressed = false;
   }

   /// <summary>
   ///    When block is performed
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnBlockPerformed(InputAction.CallbackContext context) {
      IsBlockPressed = context.ReadValueAsButton();

      // ADDED BY ABDUL: To remedy 
      if (IsBlockPressed) {
         IsHeavyAttackPressed = false;
         IsLightAttackPressed = false;
         IsMediumAttackPressed = false;
      }
   }

   /// <summary>
   ///    When block is canceled
   /// </summary>
   /// <param name="context">Reference to our movement system</param>
   private void OnBlockCanceled(InputAction.CallbackContext context) {
      IsBlockPressed = false;
   }
}