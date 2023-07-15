using System;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Attack Type struct for identifying when we've been hit by a specific attack
/// </summary>
public struct AttackType {
    private string tag;
    private bool used;
    // Tag that we'll compare to the triggers tag
    public string Tag { get => tag; set => tag = value; }
    // Whether this attack has collided with us or not
    public bool Used { get => used; set => used = value; }
}

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
    public int maxHealth = 100;
    public float activationDistance = 15;    // Added by Abdul: The distance between the player and enemy in order for the enemy to start chasing
    public float attackReliefTime = .15f;  // Added by Abdul: Time between attacks until attacking again in seconds.
    public float attackDistance = 3;        // Added by Abdul: The distance between the player and enemy in order for the enemy to start attacking

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

    // State variables
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    /// Attacked Indicators
    private bool _isAttacked;
    // Light Attacks
    private bool _receivedFirstLightAttack;
    private bool _receivedSecondLightAttack;
    private bool _receivedThirdLightAttack;
    // Medium Attacks
    private bool _receivedFirstMediumAttack;
    private bool _receivedSecondMediumAttack;
    // Special Attacks
    private bool _receivedSlamAttack;
    private AttackType[] _recievedAttack = new AttackType[6];

    // Player Attack Damages
    private int[] _playerAttackDamages = new int[6];

    // Other
    private bool _knockedDown;
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


    //// Getters and Setters
    public Rigidbody Rigidbody => _rigidbody;
    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public bool IsAttacked => _isAttacked;
    public AttackType[] RecievedAttack => _recievedAttack;

    // Other
    public bool KnockedDown { get => _knockedDown; set => _knockedDown = value; }
    public float KnockdownMeter { get => _knockdownMeter; set => _knockdownMeter = value; }
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }
    public bool Moving { get => _moving; set => _moving = value; }
    public bool Attacking { get => _attacking; set => _attacking = value; }
    public float LastAttacked { get => _lastAttacked; set => _lastAttacked = value; }
    public int Health { get => _currentHealth; set => _currentHealth = value; }

    public Material BaseMaterial { get => _baseMaterial; set => _baseMaterial = value; }
    public Material HeavyBoundsMat { get => _heavyBoundsMat; set => _heavyBoundsMat = value; }
    public Material MediumBoundsMat { get => _mediumBoundsMat; set => _mediumBoundsMat = value; }
    public Material LightBoundsMat { get => _lightBoundsMat; set => _lightBoundsMat = value; }

    // Functions
    
    void Awake() {
        // Initializing the various struct tags
        _recievedAttack[0].Tag = "FirstLightAttack";
        _recievedAttack[1].Tag = "SecondLightAttack";
        _recievedAttack[2].Tag = "ThirdLightAttack";
        _recievedAttack[3].Tag = "FirstMediumAttack";
        _recievedAttack[4].Tag = "SecondMediumAttack";
        _recievedAttack[5].Tag = "SlamAttack";

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
        _knockdownMeter = knockdownMax;

        // Other
        _lastAttacked = attackReliefTime;
        _currentHealth = maxHealth;

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
        _currentState.UpdateStates();
    }

    private void OnTriggerEnter(Collider other) {
        // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
        // be first before anything else
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (other.CompareTag(_recievedAttack[i].Tag)) {
                _recievedAttack[i].Used = true;
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
            }
            if (_recievedAttack[i].Used) {
                checkIfStillAttacked = true;
            }
        }

        _isAttacked = checkIfStillAttacked;
    }

    /// <summary>
    /// Determines the knockdown pressure depending on the type of attack used against us
    /// </summary>
    /// <returns>knockdown pressure</returns>
    public int DetermineKnockdownPressure() {
        if (_knockdownMeter <= 0) {
            _knockdownMeter = 0;
            return 0;
        }

        int pressure = 0;
        if (_recievedAttack[0].Used) {
            pressure = 40;
        } else if (_recievedAttack[1].Used) {
            pressure = 60;
        } else if (_recievedAttack[2].Used) {
            pressure = 100;
        } else if (_recievedAttack[3].Used) {
            pressure = 70;
        } else if (_recievedAttack[4].Used) {
            pressure = 80;
        } else if (_recievedAttack[5].Used) {
            pressure = 150;
        }

        Debug.Log(pressure);

        return pressure;
    }

    /// <summary>
    /// Determines the knockdown pressure depending on the type of attack used against us
    /// </summary>
    /// <returns>knockdown pressure</returns>
    public int GetPressureAndDamage() {
        int pressure = 0;
        int damage = 0;

        if (_recievedAttack[0].Used) {
            pressure = 40;
            damage = _playerAttackDamages[0];
        } else if (_recievedAttack[1].Used) {
            pressure = 60;
            damage = _playerAttackDamages[1];
        } else if (_recievedAttack[2].Used) {
            pressure = 100;
            damage = _playerAttackDamages[2];
        } else if (_recievedAttack[3].Used) {
            pressure = 70;
            damage = _playerAttackDamages[3];
        } else if (_recievedAttack[4].Used) {
            pressure = 80;
            damage = _playerAttackDamages[4];
        } else if (_recievedAttack[5].Used) {
            pressure = 150;
            damage = _playerAttackDamages[5];
        }

        Debug.Log(damage);

        if (_knockdownMeter <= 0) {
            _knockdownMeter = 0;
            return 0;
        }
        return pressure;
    }
}
