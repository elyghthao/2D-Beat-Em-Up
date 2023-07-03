using System;
using System.Collections.ObjectModel;
using UnityEngine;

public enum Attack {Light1, Light2, Light3, Medium1, Medium2, Slam}

public struct AttackType {
    private string tag;
    private bool used;
    public string Tag { get => tag; set => tag = value; }
    public bool Used { get => used; set => used = value; }
}

public class EnemyStateMachine : MonoBehaviour {
    
    //// Variables
    
    /// Inspector Arguments
    [Header("Body Pieces")]
    public GameObject body;
    [Header("Stats")]
    public int knockdownMax = 150;

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

    // Functions
    
    void Awake() {
        _states = new EnemyStateFactory(this);
        _baseMaterial = body.GetComponent<Renderer>().material;
        _rigidbody = GetComponent<Rigidbody>();
        _knockdownMeter = knockdownMax;
        _currentState = _states.Idle();
        _currentState.EnterState();
        // Init _recievedAttacks
        _recievedAttack[0].Tag = "FirstLightAttack";
        _recievedAttack[1].Tag = "SecondLightAttack";
        _recievedAttack[2].Tag = "ThirdLightAttack";
        _recievedAttack[3].Tag = "FirstMediumAttack";
        _recievedAttack[4].Tag = "SecondMediumAttack";
        _recievedAttack[5].Tag = "SlamAttack";
    }
    
    void Update() {
        _currentState.UpdateStates();
    }

    private void OnTriggerEnter(Collider other) {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        for (int i = 0; i < _recievedAttack.Length; i++) {
            if (other.CompareTag(_recievedAttack[i].Tag)) {
                _recievedAttack[i].Used = true;
                _isAttacked = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
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
