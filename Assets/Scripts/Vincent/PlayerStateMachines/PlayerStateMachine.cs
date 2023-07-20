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
/// Context file that holds important information for all player states to reference
/// </summary>
public class PlayerStateMachine : MonoBehaviour {
    
    // Inspector Arguments
    [Header("Body Elements")]
    public GameObject body;
    public int maxHealth = 100;
    
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
    
    
    [Header("Combat Stats")]
    public int knockdownMax = 150;

    // Reference variables
    private Rigidbody _rigidbody;
    private AttackBoundsManager _heavyBounds;
    private AttackBoundsManager _mediumBounds;
    private AttackBoundsManager _lightBounds;
    private Material _baseMaterial;
    private GameManager _gameManager;
    private InputSystem _inputSystem;

    // State variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    
    /// Attacked Indicators
    private bool _isAttacked;
    
    // Other Variables
    private bool _characterFlipped;
    private bool _knockedDown;
    private bool _dashing;
    private bool _isGrounded;
    private float _knockdownMeter;
    private float _stunTimer;
    public int _currentHealth;
    private AttackType[] _recievedAttack = new AttackType[6];

    // Constants
    private readonly int _zero = 0;
    

    // Getters and Setters
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Vector2 CurrentMovementInput { get => _inputSystem.CurrentMovementInput; }
    public bool IsMovementPressed { get => _inputSystem.IsMovementPressed; }
    public Material BaseMaterial { get => _baseMaterial; set => _baseMaterial = value; }
    public AttackBoundsManager HeavyBounds { get => _heavyBounds; set => _heavyBounds = value; }
    public AttackBoundsManager MediumBounds { get => _mediumBounds; set => _mediumBounds = value; }
    public AttackBoundsManager LightBounds { get => _lightBounds; set => _lightBounds = value; }
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public bool IsActionPressed { get => _inputSystem.IsActionPressed; }
    public bool IsActionHeld { get => _inputSystem.IsActionHeld; }
    public bool IsLightAttackPressed { get => _inputSystem.IsLightAttackPressed; }
    public bool IsMediumAttackPressed { get => _inputSystem.IsMediumAttackPressed; }
    public bool IsHeavyAttackPressed { get => _inputSystem.IsHeavyAttackPressed; }
    public bool IsBlockPressed { get => _inputSystem.IsBlockPressed; }
    public bool IsBlockHeld { get => _inputSystem.IsBlockHeld; }
    public bool CharacterFlipped { get => _characterFlipped; set => _characterFlipped = value; }
    public bool IsAttacked => _isAttacked;
    public bool KnockedDown { get => _knockedDown; set => _knockedDown = value; }
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public bool Dashing { get => _dashing; set => _dashing = value; }
    public float KnockdownMeter { get => _knockdownMeter; set => _knockdownMeter = value; }
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public AttackType[] RecievedAttack { get => _recievedAttack; set => _recievedAttack = value; }

    // Functions
    public void Initialize() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _inputSystem = _gameManager.InputSystem;
        _recievedAttack[(int)Attacks.LightAttack1] = new AttackType("FirstLightAttack", new Vector2(1, 10), 40, 5);
        _recievedAttack[(int)Attacks.LightAttack2] = new AttackType("SecondLightAttack", new Vector2(1, 5), 60, 15);
        _recievedAttack[(int)Attacks.LightAttack3] = new AttackType("ThirdLightAttack", new Vector2(5, 10), 100, 30);
        _recievedAttack[(int)Attacks.MediumAttack1] = new AttackType("FirstMediumAttack", new Vector2(1, 1), 70, 40);
        _recievedAttack[(int)Attacks.MediumAttack2] = new AttackType("SecondMediumAttack", new Vector2(3, 1), 80, 50);
        _recievedAttack[(int)Attacks.Slam] = new AttackType("SlamAttack", new Vector2(1, 5), 150, 50);
        
        _states = new PlayerStateFactory(this);
        
        _baseMaterial = body.GetComponent<Renderer>().material;
        _heavyBounds = heavyAttackBounds.GetComponent<AttackBoundsManager>();
        _mediumBounds = mediumAttackBounds.GetComponent<AttackBoundsManager>();
        _lightBounds = lightAttackBounds.GetComponent<AttackBoundsManager>();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        
        // enter initial state. All assignments should go before here
        _currentState = _states.Idle();
        _currentState.EnterState();
        _currentHealth = maxHealth;
    }

    /// <summary>
    /// Enables all input for the character when the PlayerStateMachine script is enabled
    /// </summary>
    private void OnEnable() {
        if (_inputSystem != null) {
            _inputSystem.EnablePlayerInput();
        }
    }

    /// <summary>
    /// Disables all input for the character when the PlayerStateMachine script is disabled
    /// </summary>
    private void OnDisable() {
        _inputSystem.DisablePlayerInput();
    }

    // Update is called once per frame
    void Update() {
        _currentState.UpdateStates();
        _isGrounded = CheckIfGrounded();
    }
    
    public bool CheckIfGrounded() {
        RaycastHit hit;
        Vector3 curPos = transform.position;
        // Debug.DrawRay(curPos, -Vector3.up * 0.3f, Color.red);
        if (Physics.Raycast(new Vector3(curPos.x, curPos.y + 0.25f, curPos.z), -transform.up * 0.3f, out hit, 1f)) {
            return true;
        }
        return false;
    }

    public void ApplyAttackStats() {
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (_recievedAttack[i].StatsApplied || !_recievedAttack[i].Used) {
                continue;
            }

            Vector2 appliedKnockback = _recievedAttack[i].KnockbackDirection;
            if (_recievedAttack[i].AttackedFromRightSide) {
                appliedKnockback = new Vector2(appliedKnockback.x * -1, appliedKnockback.y); 
            }
            Rigidbody.velocity = new Vector3(appliedKnockback.x, appliedKnockback.y, 0);
            _knockdownMeter -= _recievedAttack[i].KnockdownPressure;
            _currentHealth -= _recievedAttack[i].Damage;
            _recievedAttack[i].StatsApplied = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
        // be first before anything else
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (other.CompareTag(_recievedAttack[i].Tag)) {
                _recievedAttack[i].Used = true; 
                if (other.transform.position.x > transform.position.x) {
                    _recievedAttack[i].AttackedFromRightSide = true;
                }
                _isAttacked = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
        // be first before anything else
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        bool checkIfStillAttacked = false;
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (other.CompareTag(_recievedAttack[i].Tag)) {
                _recievedAttack[i].Used = false;
                _recievedAttack[i].AttackedFromRightSide = false;
                _recievedAttack[i].StatsApplied = false;
            }
            if (_recievedAttack[i].Used) {
                checkIfStillAttacked = true;
            }
        }

        _isAttacked = checkIfStillAttacked;
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
        // Debug.Log("Character flipped: " + _characterFlipped);
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }

    /// <summary>
    /// Adds health to the player
    /// </summary>
    public void HealCharacter(int addedHealth) {
        if (addedHealth <= 0) { return; }
        _currentHealth += addedHealth;
        if (_currentHealth > maxHealth) {
            _currentHealth = maxHealth;
        }
    }
}
