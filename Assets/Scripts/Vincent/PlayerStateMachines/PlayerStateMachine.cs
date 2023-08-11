using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
   [Header("References")]
   public GameObject body;
   [Header("---")]
   public GameObject mediumFirstFollowupAttackBounds;
   public GameObject lightFirstFollowupAttackBounds;
   public GameObject lightSecondFollowupAttackBounds;
   [Header("---")]
   public GameObject lightAttackBounds;
   public GameObject mediumAttackBounds;
   public GameObject heavyAttackBounds;
   
   [Header("Variables")]
   public int framesPerSecond;
   [Header("---")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public float heavyStartupFrames = 10;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public float heavyActiveFrames = 15;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public float heavyRecoveryFrames = 44;

   [Header("---")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int mediumStartupFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int mediumActiveFrames = 14;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int mediumRecoveryFrames = 32;
   [Header("---")]
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

   [Header("---")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light1StartupFrames = 7;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light1ActiveFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light1RecoveryFrames = 23;

   [Header("---")]
   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light2StartupFrames = 7;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light2ActiveFrames = 9;

   [Tooltip("Must be between 0 and mediumFrameCount + 1, cannot overlap with other frames")]
   public int light2RecoveryFrames = 23;
   
   [Header("---")]

   public int maxHealth = 100;
   public bool gotHealed;
   public float stamina;
   
   [Tooltip("Time in seconds it takes for stamina to regenerate after performing an action")]
   public float staminaRegenDelay;
   
   [Tooltip("The amount of stamina regenerated per second")]
   public float staminaRegenRate;
   
   [Tooltip("How much knockdown pressure this character can take before entering the knockdown state")]
   public int knockdownMax = 150;

   [Tooltip("How much time in seconds is given to initiate a followup attack")]
   public float attackFollowupThreshold = 0.75f;
   public float movementSpeed;
   [Tooltip("How long, in seconds, the player stays invulnerable after recovery")]
   public float invulnerabilityTime = 0.1f;

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

   public int CurrentHealth { get; set; }

   //public AttackType[] RecievedAttack { get; set; } = new AttackType[6];
   private Dictionary<GameObject, AttackType> _receivedAttacks = new Dictionary<GameObject, AttackType>();
   public PowerupSystem PowerupSystem => GameManager.Instance.PowerupSystem;
   public PlayerBaseState QueuedAttack { get; set; }
   public float FollowupTimer { get; set; }

   public bool CanQueueAttacks { get; set; }

   public string MostRecentAttack { get; set; }

   public bool FinishedInitialization { get; private set; }

   public SpriteEffects SpriteEffects { get; private set; }

   public float Stamina { get; set; }
   public bool StaminaRegenAllowed { get; set; }
   public float StaminaRegenDelay { get; private set; }
   public bool IsDead {get; private set;}
   public int ConsecutiveSmackedCount { get; set; }
   public bool SpendInvulnerabilityTime { get; set; }
   private float invuln;

   private int ConsecutiveSmacks { get; set; }
   // Functions

   private void Awake() {
      InputSys = GameManager.Instance.gameObject.GetComponent<InputSystem>();
      // RecievedAttack[(int)Attacks.LightAttack1] = new AttackType("FirstLightAttack", new Vector2(10, 500), 40, 5);
      // RecievedAttack[(int)Attacks.LightAttack2] = new AttackType("SecondLightAttack", new Vector2(50, 250), 60, 15);
      // RecievedAttack[(int)Attacks.LightAttack3] = new AttackType("ThirdLightAttack", new Vector2(150, 500), 100, 30);
      // RecievedAttack[(int)Attacks.MediumAttack1] = new AttackType("FirstMediumAttack", new Vector2(50, 500), 70, 40);
      // RecievedAttack[(int)Attacks.MediumAttack2] = new AttackType("SecondMediumAttack", new Vector2(800, 100), 80, 50);
      // RecievedAttack[(int)Attacks.Slam] = new AttackType("SlamAttack", new Vector2(400, 800), 150, 50);

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
      Stamina = stamina;
      StaminaRegenDelay = staminaRegenDelay;
      invuln = invulnerabilityTime;

      // enter initial state. All assignments should go before here
      _states = new PlayerStateFactory(this);
      CurrentState = _states.Idle();
      CurrentState.EnterState();
      FinishedInitialization = true;
   }

   private void Start(){
      KnockdownMeter = knockdownMax;
      IsDead = false;
   }

   // Update is called once per frame
   private void Update() {
      // Debug.Log("knockdown meter: " + KnockdownMeter);
      if(KnockdownMeter < knockdownMax) {//knockdown meter can regen
            KnockdownMeter += (3*Time.deltaTime);
      }
      IsGrounded = CheckIfGrounded();
      CurrentState.UpdateStates();
      if (IsGrounded && ConsecutiveSmacks > 0) {
         ConsecutiveSmacks = 0;
      }
      if (FollowupTimer > 0) {
         FollowupTimer -= Time.deltaTime;
         //Debug.Log("Followup Timer: " + FollowupTimer);
      }

      // Debug.Log(CurrentState + " sub: " + CurrentState.CurrentSubState);
      // Debug.Log("Attacking? - " + IsLightAttackPressed);
      if (SpendInvulnerabilityTime && invuln >= invulnerabilityTime) {
         invuln -= Time.deltaTime;
      } else if (SpendInvulnerabilityTime && invuln <= 0) {
         SpendInvulnerabilityTime = false;
         invuln = invulnerabilityTime;
      }

      if (IsGrounded && ConsecutiveSmackedCount > 0) {
         ConsecutiveSmackedCount = 0;
      }

      if (StaminaRegenAllowed) RegenerateStamina();
      else StaminaRegenDelay = staminaRegenDelay;
   }

   private void FixedUpdate() {
      IsGrounded = CheckIfGrounded();
      CurrentState.FixedUpdateStates();
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
      //ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
      AttackBoundsManager otherAttackManager;
      if (other.TryGetComponent<AttackBoundsManager>(out otherAttackManager)) {
         if (CurrentState.CurrentSubState != null && 
             (CurrentState.CurrentSubState.ToString() == "PlayerBlockState" ||
             CurrentState.CurrentSubState.ToString() == "PlayerRecoveryState")) return;
         if (_receivedAttacks.ContainsKey(other.gameObject)) return;
         
         AttackType receivedAttack = new AttackType(otherAttackManager.selectedAttack.ToString(), 
            otherAttackManager.knockback, otherAttackManager.pressure, otherAttackManager.damage);
         
         if (other.transform.parent.position.x > transform.position.x) receivedAttack.AttackedFromRightSide = true;
         _receivedAttacks[other.gameObject] = receivedAttack;
         IsAttacked = true;
      }
   }

   // private void OnTriggerExit(Collider other) {
   //    // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
   //    // be first before anything else
   //    ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
   //    //bool checkIfStillAttacked = false;
   //    if (_receivedAttacks.ContainsKey(other.gameObject)) {
   //       _receivedAttacks.Remove(other.gameObject);
   //    }
   //    // for (int i = 0; i < _recievedAttack.Length; i++) {
   //    //     if (other.CompareTag(_recievedAttack[i].Tag)) {
   //    //         _recievedAttack[i].Used = false;
   //    //         _recievedAttack[i].AttackedFromRightSide = false;
   //    //         _recievedAttack[i].StatsApplied = false;
   //    //     }
   //    //     if (_recievedAttack[i].Used) {
   //    //         checkIfStillAttacked = true;
   //    //     }
   //    // }
   //  
   //    // _isAttacked = checkIfStillAttacked;
   //    IsAttacked = false;
   // }

   private IEnumerator SafeOnEnable() {
      while (InputSys == null || InputSys.EmptyPlayerInput) yield return null;
      InputSys.EnablePlayerInput();
   }

   public bool CheckIfGrounded() {
      RaycastHit hit;
      var curPos = transform.position;
      // Debug.DrawRay(curPos, -Vector3.up * 0.3f, Color.red);
      if (Physics.Raycast(new Vector3(curPos.x, curPos.y + 0.25f, curPos.z), -transform.up * 0.3f, out hit, .2f))
         if(hit.collider.CompareTag("Ground"))
                return true;
      return false;
   }

   public List<string> ApplyAttackStats() {
      List<string> recievedAttackNames = new List<string>();
      // Debug.Log(InputSys.IsBlockHeld);
      foreach (AttackType i in _receivedAttacks.Values) {
         if (KnockdownMeter > 0) {
            KnockdownMeter -= i.KnockdownPressure;
         }
         if (KnockdownMeter < 0) {
            KnockedDown = true;
         }
         if (KnockedDown) {
            Vector2 appliedKnockback = i.KnockbackDirection;
            if (i.AttackedFromRightSide) {
               appliedKnockback = new Vector2(appliedKnockback.x * -1, appliedKnockback.y);
            }

            appliedKnockback = new Vector2(appliedKnockback.x * ConsecutiveSmacks, appliedKnockback.y);
            // appliedKnockback = new Vector2(appliedKnockback.x * 8, appliedKnockback.y);//elygh added this to increase knockback
            Rigidbody.velocity = Vector3.zero;
            // Debug.Log("Knockback Applied: " + appliedKnockback + " from " + i);
            Rigidbody.AddForce(new Vector3(appliedKnockback.x, appliedKnockback.y, 0));
            // Debug.Log("applied knockback: " + appliedKnockback.x + "     player x scale:" + transform.localScale.x);
            if((appliedKnockback.x < 0 && transform.localScale.x < 0) 
               || (appliedKnockback.x > 0 && transform.localScale.x > 0)){
               FlipCharacter();
            }
         } else {
            KnockdownMeter -= i.KnockdownPressure;
         }
         CurrentHealth -= i.Damage;
         recievedAttackNames.Add(i.Name);
         ConsecutiveSmacks++;
      }
      _receivedAttacks.Clear();
      IsAttacked = false;
      return recievedAttackNames;
   }

   /// <summary>
   ///    Calculates the speed of our character
   /// </summary>
   public void SpeedControl() {
      var playerVelocity = Rigidbody.velocity;
      var flatVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);
      // Debug.Log("current player velocity: " + playerVelocity);
      // limit velocity if needed
      if (flatVelocity.magnitude > movementSpeed) {
         var limitedVelocity = flatVelocity.normalized * movementSpeed;
         // Debug.Log("new player velocity: " + limitedVelocity);
         GetComponent<Rigidbody>().velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z*2);
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
      CurrentHealth += addedHealth;
      if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;
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

   private void RegenerateStamina() {
      StaminaRegenDelay -= Time.deltaTime;
      if (StaminaRegenDelay <= 0) {
         Stamina += staminaRegenRate * Time.deltaTime;
         if (Stamina >= stamina) {
            if (Stamina < 1000) { // Won't reset stamina if we're in cheat mode for stamina
               Stamina = stamina;
            }
            StaminaRegenDelay = staminaRegenDelay;
            StaminaRegenAllowed = false;
         }
      }
   }
   public void SetDead() {
      Scene current_scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(current_scene.name);
    }


   public IEnumerator DeathTimeDelay(float waitTime){
        yield return new WaitForSeconds(waitTime);
        IsDead = true;
        yield return new WaitForSeconds(0.5f);
        this.SetDead();
   }

   public GameObject InstantiatePrefab(GameObject obj) {
      return Instantiate(obj);
   }
   
   public void ClearRecievedAttacks() {
      _receivedAttacks.Clear();
      IsAttacked = false;
   }
}