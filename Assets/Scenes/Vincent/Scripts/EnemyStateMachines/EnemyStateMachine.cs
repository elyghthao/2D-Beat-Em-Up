using UnityEngine;

public enum Attack {Light1, Light2, Light3, Medium1, Medium2, Slam}

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
    
    private bool[] _recievedAttack = new bool[6];

    // Other
    private bool _checkTrigger;
    private int _knockdownMeter;
    private float _stunTimer;


    //// Getters and Setters
    public Material BaseMaterial => _baseMaterial;
    public Rigidbody Rigidbody => _rigidbody;
    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public bool IsAttacked => _isAttacked;
    // Light Attacks
    public bool ReceivedFirstLightAttack => _receivedFirstLightAttack;
    public bool ReceivedSecondLightAttack => _receivedSecondLightAttack;
    public bool ReceivedThirdLightAttack => _receivedThirdLightAttack;
    // Medium Attacks
    public bool ReceivedFirstMediumAttack => _receivedFirstMediumAttack;
    public bool ReceivedSecondMediumAttack => _receivedSecondMediumAttack;
    // Special Attacks
    public bool ReceivedSlamAttack => _receivedSlamAttack;
    public bool[] RecievedAttack => _recievedAttack;
    // Other
    public int KnockdownMeter { get => _knockdownMeter; set => _knockdownMeter = value; }
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }

    // Functions
    
    void Awake() {
        _states = new EnemyStateFactory(this);
        _baseMaterial = body.GetComponent<Renderer>().material;
        _rigidbody = GetComponent<Rigidbody>();
        _knockdownMeter = knockdownMax;
        _currentState = _states.Idle();
        _currentState.EnterState();
    }
    
    void Update() {
        _currentState.UpdateStates();
        if (_checkTrigger) {
            bool triggerChecker = false;
            for (int i = 0; i < _recievedAttack.Length; i++) {
                if (_recievedAttack[i]) {
                    triggerChecker = true;
                }
                _recievedAttack[i] = false;
            }
            _isAttacked = triggerChecker;
            if (!triggerChecker) {
                _checkTrigger = false;
                _isAttacked = false;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        _checkTrigger = true;
        // Light Attacks
        if (other.CompareTag("FirstLightAttack")) {
            _recievedAttack[(int)Attack.Light1] = true;
        } else if (other.CompareTag("SecondLightAttack")) {
            _recievedAttack[(int)Attack.Light2] = true;
        } else if (other.CompareTag("ThirdLightAttack")) {
            _recievedAttack[(int)Attack.Light3] = true;
        }
        // Medium Attacks
        else if (other.CompareTag("FirstMediumAttack")) {
            _recievedAttack[(int)Attack.Medium1] = true;
        } else if (other.CompareTag("SecondMediumAttack")) {
            _recievedAttack[(int)Attack.Medium2] = true;
        }
        // Special
        else if (other.CompareTag("SlamAttack")) {
            _recievedAttack[(int)Attack.Slam] = true;
        }
    }
}
