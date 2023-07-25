using System.Collections;
using UnityEngine;

public enum Attacks {
   LightAttack1,
   LightAttack2,
   LightAttack3,
   MediumAttack1,
   MediumAttack2,
   Slam
}


/// <summary>
///    Context file that holds important information for all player states to reference
/// </summary>
public class PlayerStateMachine : MonoBehaviour {
   // Inspector Arguments
   [Header("Body Elements")] public GameObject body;

   public int maxHealth = 100;
   public bool gotHealed = false;

   [Header("Attack Boundaries")] public GameObject heavyAttackBounds;

   public GameObject mediumAttackBounds;
   public GameObject mediumFirstFollowupAttackBounds;
   public GameObject lightAttackBounds;
   public GameObject lightFirstFollowupAttackBounds;
   public GameObject lightSecondFollowupAttackBounds;

   [Header("FrameData")] public int framesPerSecond;

   [Header("HeavyAttack")] [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public float heavyStartupFrames = 10;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public float heavyActiveFrames = 15;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public float heavyRecoveryFrames = 44;

   [Header("MediumAttack")] [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int mediumStartupFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int mediumActiveFrames = 14;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int mediumRecoveryFrames = 32;

   [Header("MediumFirstFollowupAttack")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int medium1StartupFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int medium1ActiveFrames = 14;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int medium1RecoveryFrames = 32;

   [Header("LightAttack")] [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int lightStartupFrames = 7;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int lightActiveFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int lightRecoveryFrames = 23;

   [Header("LightFirstFollowupAttack")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light1StartupFrames = 7;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light1ActiveFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light1RecoveryFrames = 23;

   [Header("LightsecondFollowupAttack")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light2StartupFrames = 7;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light2ActiveFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light2RecoveryFrames = 23;

   [Header("Combat Stats")] public int knockdownMax = 150;

   [Tooltip("How much time in seconds is given to initiate a followup attack")]
   public float attackFollowupThreshold = 0.75f;

   public int currentHealth;

   [Header("Movement")] public float movementSpeed;


   // Constants
   private readonly int _zero = 0;

   // Reference variables
   private PlayerStateFactory _states;

   // Getters and Setters
   public Vector2 CurrentMovementInput {
      get {
         if (InputSys == null) return new Vector2(0, 0);
         return InputSys.CurrentMovementInput;
      }
   }

   public bool IsMovementPressed {
      get {
         if (InputSys == null) return false;
         return InputSys.IsMovementPressed;
      }
   }
   public bool IsActionPressed {
      get {
         if (InputSys == null) return false;
         return InputSys.IsActionPressed;
      }
   }
   public bool IsActionHeld {
      get {
         if (InputSys == null) return false;
         return InputSys.IsActionHeld;
      }
   }
   public bool IsLightAttackPressed {
      get {
         if (InputSys == null) return false;
         return InputSys.IsLightAttackPressed;
      }
   }
   public bool IsMediumAttackPressed {
      get {
         if (InputSys == null) return false;
         return InputSys.IsMediumAttackPressed;
      }
   }
   public bool IsPowerupPressed {
      get {
         if (InputSys == null) return false;
         return InputSys.IsHeavyAttackPressed;
      }
   }
   public bool IsBlockPressed {
      get {
         if (InputSys == null) return false;
         return InputSys.IsBlockPressed;
      }
   }
   public bool IsBlockHeld {
      get {
         if (InputSys == null) return false;
         return InputSys.IsBlockHeld;
      }
   }
   public PlayerBaseState CurrentState { get; set; }
   public Material BaseMaterial { get; set; }
   public AttackBoundsManager HeavyBounds { get; set; }
   public AttackBoundsManager MediumBounds { get; set; }
   public AttackBoundsManager MediumFirstFollowupBounds { get; private set; }
   public AttackBoundsManager LightBounds { get; set; }
   public AttackBoundsManager LightFirstFollowupBounds { get; private set; }
   public AttackBoundsManager LightSecondFollowupBounds { get; private set; }
   public Rigidbody Rigidbody { get; set; }
   public InputSystem InputSys { get; set; }
   public bool CharacterFlipped { get; set; }
   /// Attacked Indicators
   public bool IsAttacked { get; private set; }
   public bool KnockedDown { get; set; }
   public bool IsGrounded { get; set; }
   public bool Dashing { get; set; }
   public float KnockdownMeter { get; set; }
   public float StunTimer { get; set; }

   public int CurrentHealth {
      get => currentHealth;
      set => currentHealth = value;
   }

   public AttackType[] RecievedAttack { get; set; } = new AttackType[6];
   public PowerupSystem PowerupSystem => GameManager.Instance.PowerupSystem;
   public PlayerBaseState QueuedAttack { get; set; }
   public float FollowupTimer { get; set; }

   public bool CanQueueAttacks { get; set; }

   public string MostRecentAttack { get; set; }

   public bool FinishedInitialization { get; private set; }

   public SpriteEffects SpriteEffects { get; private set; }

   // Functions

   private void Awake() {
      InputSys = GameManager.Instance.gameObject.GetComponent<InputSystem>();
      RecievedAttack[(int) Attacks.LightAttack1] = new AttackType("FirstLightAttack", new Vector2(1, 10), 40, 5);
      RecievedAttack[(int) Attacks.LightAttack2] = new AttackType("SecondLightAttack", new Vector2(1, 5), 60, 15);
      RecievedAttack[(int) Attacks.LightAttack3] = new AttackType("ThirdLightAttack", new Vector2(5, 10), 100, 30);
      RecievedAttack[(int) Attacks.MediumAttack1] = new AttackType("FirstMediumAttack", new Vector2(1, 1), 70, 40);
      RecievedAttack[(int) Attacks.MediumAttack2] = new AttackType("SecondMediumAttack", new Vector2(3, 1), 80, 50);
      RecievedAttack[(int) Attacks.Slam] = new AttackType("SlamAttack", new Vector2(1, 5), 150, 50);

      BaseMaterial = body.GetComponent<Renderer>().material;
      HeavyBounds = heavyAttackBounds.GetComponent<AttackBoundsManager>();
      MediumBounds = mediumAttackBounds.GetComponent<AttackBoundsManager>();
      MediumFirstFollowupBounds = mediumFirstFollowupAttackBounds.GetComponent<AttackBoundsManager>();
      LightBounds = lightAttackBounds.GetComponent<AttackBoundsManager>();
      LightFirstFollowupBounds = lightFirstFollowupAttackBounds.GetComponent<AttackBoundsManager>();
      LightSecondFollowupBounds = lightSecondFollowupAttackBounds.GetComponent<AttackBoundsManager>();

      SpriteEffects = GetComponent<SpriteEffects>();
      Rigidbody = GetComponent<Rigidbody>();
      Rigidbody.freezeRotation = true;

      CurrentHealth = maxHealth;

      // enter initial state. All assignments should go before here
      _states = new PlayerStateFactory(this);
      CurrentState = _states.Idle();
      CurrentState.EnterState();
      FinishedInitialization = true;
   }

   // Update is called once per frame
   private void Update() {
      CurrentState.UpdateStates();
      IsGrounded = CheckIfGrounded();
      if (FollowupTimer > 0) {
         FollowupTimer -= Time.deltaTime;
         //Debug.Log("Followup Timer: " + FollowupTimer);
      }
   }

   /// <summary>
   ///    Enables all input for the character when the PlayerStateMachine script is enabled
   /// </summary>
   private void OnEnable() {
      StartCoroutine(SafeOnEnable());
   }

   /// <summary>
   ///    Disables all input for the character when the PlayerStateMachine script is disabled
   /// </summary>
   private void OnDisable() {
      InputSys.DisablePlayerInput();
   }

   private void OnDestroy() {
      GameManager.Instance.PlayerRef = null;
   }

   private void OnTriggerEnter(Collider other) {
      // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
      // be first before anything else
      ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
      for (var i = 0; i < RecievedAttack.Length; i++)
         if (other.CompareTag(RecievedAttack[i].Tag)) {
            RecievedAttack[i].Used = true;
            if (other.transform.position.x > transform.position.x) RecievedAttack[i].AttackedFromRightSide = true;
            IsAttacked = true;
         }
   }

   private void OnTriggerExit(Collider other) {
      // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
      // be first before anything else
      ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
      var checkIfStillAttacked = false;
      for (var i = 0; i < RecievedAttack.Length; i++) {
         if (other.CompareTag(RecievedAttack[i].Tag)) {
            RecievedAttack[i].Used = false;
            RecievedAttack[i].AttackedFromRightSide = false;
            RecievedAttack[i].StatsApplied = false;
         }

         if (RecievedAttack[i].Used) checkIfStillAttacked = true;
      }

      IsAttacked = checkIfStillAttacked;
   }

   private IEnumerator SafeOnEnable() {
      while (InputSys == null || InputSys.EmptyPlayerInput) yield return null;
      InputSys.EnablePlayerInput();
   }

   public bool CheckIfGrounded() {
      RaycastHit hit;
      var curPos = transform.position;
      // Debug.DrawRay(curPos, -Vector3.up * 0.3f, Color.red);
      if (Physics.Raycast(new Vector3(curPos.x, curPos.y + 0.25f, curPos.z), -transform.up * 0.3f, out hit, 1f))
         return true;
      return false;
   }

   public void ApplyAttackStats() {
      for (var i = 0; i < RecievedAttack.Length; i++) {
         if (RecievedAttack[i].StatsApplied || !RecievedAttack[i].Used) continue;

         var appliedKnockback = RecievedAttack[i].KnockbackDirection;
         if (RecievedAttack[i].AttackedFromRightSide)
            appliedKnockback = new Vector2(appliedKnockback.x * -1, appliedKnockback.y);
         Rigidbody.velocity = new Vector3(appliedKnockback.x, appliedKnockback.y, 0);
         KnockdownMeter -= RecievedAttack[i].KnockdownPressure;
         currentHealth -= RecievedAttack[i].Damage;
         RecievedAttack[i].StatsApplied = true;
      }
   }

   /// <summary>
   ///    Calculates the speed of our character
   /// </summary>
   public void SpeedControl() {
      var playerVelocity = Rigidbody.velocity;
      var flatVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);

      // limit velocity if needed
      if (flatVelocity.magnitude > movementSpeed) {
         var limitedVelocity = flatVelocity.normalized * movementSpeed;
         GetComponent<Rigidbody>().velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z);
      }
   }

   /// <summary>
   ///    Flips our character object to face the other direction
   /// </summary>
   public void FlipCharacter() {
      CharacterFlipped = !CharacterFlipped;
      // Debug.Log("Character flipped: " + _characterFlipped);
      transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));

      // Effect
      int effectNumber = Random.Range(1, 3);
      if (effectNumber == 1) {
         SpriteEffects.doEffect("Direction", CharacterFlipped); 
      } else {
         SpriteEffects.doEffect("Direction2", CharacterFlipped); 
      }

      // NEW FLIP SYSTEM BELOW
      // if (!CharacterFlipped) {
      //     transform.localEulerAngles = new Vector3(0, 0, 0);
      // } else {
      //     transform.localEulerAngles = new Vector3(0, 180, 0);
      // }
   }

   /// <summary>
   ///    Adds health to the player
   /// </summary>
   public void HealCharacter(int addedHealth) {
      gotHealed = true;
      if (addedHealth <= 0) return;
      currentHealth += addedHealth;
      if (currentHealth > maxHealth) currentHealth = maxHealth;
   }

   public int FrameState(AttackBoundsManager bounds, float currentFrame, int startup, int active, int recovery) {
      // Displays the current state of the attack frames.
      // Green is startup frames: No damage is given in this phase
      // Red is active frames: Damage can be given in this phase
      // Blue is recovery frames: No damage given in this phase
      if (currentFrame <= startup) {
         bounds.SetMatColor(Color.green);
         return 0;
      }

      if (currentFrame <= active) {
         bounds.SetMatColor(Color.red);
         bounds.SetColliderActive(true);
         return 1;
      }

      if (currentFrame <= recovery) {
         bounds.SetMatColor(Color.blue);
         bounds.SetColliderActive(false);
         return 2;
      }

      return 3;
   }

   public void ResetAttackQueue() {
      QueuedAttack = null;
      CanQueueAttacks = false;
   }
}