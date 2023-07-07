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
    [Header("Player State Machine Reference")]
    public PlayerStateMachine currentPlayerMachine;
    [Header("Enemy Settings")]
    public EnemyType enemyType;
    public float activationDistance;    // Added by Abdul: The distance between the player and enemy in order for the enemy to start chasing

    // Reference Variables
    private Material _baseMaterial;
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

    // Other
    private bool _knockedDown;
    private float _knockdownMeter;
    private float _stunTimer;

    // Movement
    private bool _moving;


    //// Getters and Setters
    public Material BaseMaterial => _baseMaterial;
    public Rigidbody Rigidbody => _rigidbody;
    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public bool IsAttacked => _isAttacked;
    public AttackType[] RecievedAttack => _recievedAttack;
    // Other
    public bool KnockedDown { get => _knockedDown; set => _knockedDown = value; }
    public float KnockdownMeter { get => _knockdownMeter; set => _knockdownMeter = value; }
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }
    public bool Moving { get => _moving; set => _moving = value; }

    // Functions
    
    void Awake() {
        // Initializing the various struct tags
        _recievedAttack[0].Tag = "FirstLightAttack";
        _recievedAttack[1].Tag = "SecondLightAttack";
        _recievedAttack[2].Tag = "ThirdLightAttack";
        _recievedAttack[3].Tag = "FirstMediumAttack";
        _recievedAttack[4].Tag = "SecondMediumAttack";
        _recievedAttack[5].Tag = "SlamAttack";
        
        _states = new EnemyStateFactory(this);
        _baseMaterial = body.GetComponent<Renderer>().material;
        _rigidbody = GetComponent<Rigidbody>();
        _knockdownMeter = knockdownMax;
        
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
        return pressure;
    }
}
