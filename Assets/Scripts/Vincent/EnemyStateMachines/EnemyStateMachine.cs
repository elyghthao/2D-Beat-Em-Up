using System;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Attack Type struct for identifying when we've been hit by a specific attack
/// </summary>
/*public struct AttackType {
    private string tag;
    private bool used;
    // Tag that we'll compare to the triggers tag
    public string Tag { get => tag; set => tag = value; }
    // Whether this attack has collided with us or not
    public bool Used { get => used; set => used = value; }
}*/

/// <summary>
/// Context file that holds important information for all enemy states to reference
/// </summary>
public class EnemyStateMachine : MonoBehaviour {
    /// <summary>
    /// For identifying the type of enemy this enemy is representing
    /// Added by Abdul
    /// </summary>
    public enum EnemyType {
        Heavy,
    };

    //// Variables

    /// Inspector Arguments
    [Header("Body Pieces")]
    public GameObject body;
    [Header("Stats")]
    public int knockdownMax = 150;
    [Header("Player Information")]
    public PlayerStateMachine currentPlayerMachine;
    [Header("Enemy Settings")]
    public EnemyType enemyType;
    public int maxHealth = 100;             // Added by Abdul: Max health of the enemy
    public float activationDistance = 15;   // Added by Abdul: The distance between the player and enemy in order for the enemy to start chasing
    public float attackReliefTime = .15f;  // Added by Abdul: Time between attacks until attacking again in seconds.
    public float attackDistance = 3;       // Added by Abdul: The distance between the player and enemy in order for the enemy to start attacking

    // Attacks
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

    // Reference Variables
    private Rigidbody _rigidbody;
    private AttackBoundsManager _heavyBounds;
    private AttackBoundsManager _mediumBounds;
    private AttackBoundsManager _lightBounds;

    // State variables
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    /// Attacked Indicators
    private bool _isAttacked;
    private AttackType[] _recievedAttack = new AttackType[6];

    // Player Attack Damages
    private int[] _playerAttackDamages = new int[6];

    // Other
    private bool _knockedDown;
    private bool _isGrounded;
    private float _knockdownMeter;
    private float _stunTimer;
    private int _currentHealth;

    // Movement and Attack Info
    private bool _moving;
    private bool _attacking;
    private float _lastAttacked;

    // Materials
    private Material _baseMaterial;
    private Material _heavyBoundsMat;
    private Material _mediumBoundsMat;
    private Material _lightBoundsMat;

    // [TEMP FIX]
    private bool _attackDebounce;       // [TEMP FIX]: Used to prevent the "doubling" glitch of player attacking the enemy
    private int _attackDLength = 5;     // [TEMP FIX]: How long each debounce should last (in FPS)
    private int _attackCTime = 0;       // [TEMP FIX]: Current time of the debounce (in FPS)

    //// Getters and Setters
    public Rigidbody Rigidbody => _rigidbody;
    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public bool IsAttacked => _isAttacked;
    public AttackType[] RecievedAttack => _recievedAttack;
    public AttackBoundsManager HeavyBounds { get => _heavyBounds; set => _heavyBounds = value; }
    public AttackBoundsManager MediumBounds { get => _mediumBounds; set => _mediumBounds = value; }
    public AttackBoundsManager LightBounds { get => _lightBounds; set => _lightBounds = value; }
    public bool Moving { get => _moving; set => _moving = value; }
    public bool Attacking { get => _attacking; set => _attacking = value; }
    public float LastAttacked { get => _lastAttacked; set => _lastAttacked = value; }
    public int Health { get => _currentHealth; set => _currentHealth = value; }

    public Material BaseMaterial { get => _baseMaterial; set => _baseMaterial = value; }
    public Material HeavyBoundsMat { get => _heavyBoundsMat; set => _heavyBoundsMat = value; }
    public Material MediumBoundsMat { get => _mediumBoundsMat; set => _mediumBoundsMat = value; }
    public Material LightBoundsMat { get => _lightBoundsMat; set => _lightBoundsMat = value; }
    public bool KnockedDown { get => _knockedDown; set => _knockedDown = value; }
    
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public float KnockdownMeter { get => _knockdownMeter; set => _knockdownMeter = value; }
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    // Functions
    void Awake() {
        _recievedAttack[(int)Attacks.LightAttack1] = new AttackType("FirstLightAttack", new Vector2(10, 500), 40, 5);
        _recievedAttack[(int)Attacks.LightAttack2] = new AttackType("SecondLightAttack", new Vector2(10, 250), 60, 15);
        _recievedAttack[(int)Attacks.LightAttack3] = new AttackType("ThirdLightAttack", new Vector2(50, 500), 100, 30);
        _recievedAttack[(int)Attacks.MediumAttack1] = new AttackType("FirstMediumAttack", new Vector2(10, 500), 70, 40);
        _recievedAttack[(int)Attacks.MediumAttack2] = new AttackType("SecondMediumAttack", new Vector2(800, 100), 80, 50);
        _recievedAttack[(int)Attacks.Slam] = new AttackType("SlamAttack", new Vector2(50, 800), 150, 50);

        // Initializing the player attack damages
        _playerAttackDamages[0] = 5;
        _playerAttackDamages[1] = 15;
        _playerAttackDamages[2] = 30;
        _playerAttackDamages[3] = 40;
        _playerAttackDamages[4] = 50;
        _playerAttackDamages[5] = 50;

        _states = new EnemyStateFactory(this);
        _baseMaterial = body.GetComponent<Renderer>().material;
        _rigidbody = GetComponent<Rigidbody>();
        _heavyBounds = heavyAttackBounds.GetComponent<AttackBoundsManager>();
        _mediumBounds = mediumAttackBounds.GetComponent<AttackBoundsManager>();
        _lightBounds = lightAttackBounds.GetComponent<AttackBoundsManager>();

        // Other
        _lastAttacked = attackReliefTime;
        _currentHealth = maxHealth;
        _knockdownMeter = knockdownMax;

        // Materials
        _baseMaterial = body.GetComponent<Renderer>().material;
        _heavyBoundsMat = heavyAttackBounds.GetComponent<Renderer>().material;
        _mediumBoundsMat = mediumAttackBounds.GetComponent<Renderer>().material;
        _lightBoundsMat = lightAttackBounds.GetComponent<Renderer>().material;

        // Begins the initial state. All Awake code should go before here unless you want it defined after the initial
        // states EnterState()
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update() {
        _isGrounded = CheckIfGrounded();
        _currentState.UpdateStates();

        // [TEMP FIX] All the lines of code below are to fix the doubling of the enemy getting attacked
        if (_attackDebounce) {
            if (_attackCTime > _attackDLength) {
                _attackDebounce = false;
                _attackCTime = 0;
            } else {
                _attackCTime++;
            }
        }
    }
    
    public bool CheckIfGrounded()
    {
        RaycastHit hit;
        Vector3 curPos = transform.position;
        Debug.DrawRay(curPos, -Vector3.up * 0.5f, Color.red);
        if (Physics.Raycast(new Vector3(curPos.x, curPos.y + 0.1f, curPos.z), -transform.up * 0.3f, out hit, 1f)) {
            if(hit.collider.CompareTag("Ground"))
                return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
        // be first before anything else
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (other.CompareTag(_recievedAttack[i].Tag)) {
                Debug.Log(i + " has collided");
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
                Debug.Log(i + " is no longer colliding");
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

    public void ApplyAttackStats() {
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (_recievedAttack[i].StatsApplied || !_recievedAttack[i].Used) {
                continue;
            }

            Vector2 appliedKnockback = _recievedAttack[i].KnockbackDirection;
            if (_recievedAttack[i].AttackedFromRightSide) {
                appliedKnockback = new Vector2(appliedKnockback.x * -1, appliedKnockback.y);
            }
            _rigidbody.velocity = Vector3.zero;
            Debug.Log("Knockback Applied: " + appliedKnockback + " from " + i);
            _rigidbody.AddForce(new Vector3(appliedKnockback.x, appliedKnockback.y, 0));
            _knockdownMeter -= _recievedAttack[i].KnockdownPressure;
            _currentHealth -= _recievedAttack[i].Damage;
            _recievedAttack[i].StatsApplied = true;
        }
    }
}
