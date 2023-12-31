using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

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
    // Static Variables
    public static int leftEnemies = 0;
    public static int leftPursuingEnemies = 0;
    public static int leftPursuingMax = 2;

    public static int rightEnemies = 0;
    public static int rightPursuingEnemies = 0;
    public static int rightPursuingMax = 2;

    // Static Variables Getter/Setter

    /// <summary>
    /// For identifying the type of enemy this enemy is representing
    /// Added by Abdul
    /// SCRIPTS: EnemyAttackingState, EnemyMovingState
    /// </summary>
    public enum EnemyType {
        Heavy,
        Medium,
        Light,
        Boss,
    };

    /// <summary>
    /// For identifying which side this enemy is going to go towards
    /// SCRIPTS: EnemyAttackingState, EnemyMovingState
    /// </summary>
    public enum FlankType {
        Left,
        Right,
        Boss
    };

    //// Variables

    /// Inspector Arguments
    [Header("Body Pieces")]
    public GameObject body;
    [Header("Stats")]
    public int knockdownMax = 150;
    [Header("Enemy Settings")]
    public EnemyType enemyType;
    public int maxHealth = 100;             // Added by Abdul: Max health of the enemy
    public float activationDistance = 15;   // Added by Abdul: The distance between the player and enemy in order for the enemy to start chasing
    public float attackReliefTime = .15f;  // Added by Abdul: Time between attacks until attacking again in seconds.
    public float attackDistance = 3.25f;       // Added by Abdul: The distance between the player and enemy in order for the enemy to start attacking
    public float zAttackDistance = .9f;    // Added by Abdul: The distance between the player and enemy in order for the enemy to start attacking on the z plane
    public float movementSpeed = 5;        // Added by Abdul: The movement speed of the enemy
    public float distanceGoal = 2.65f;     // Added by Abdul: Distance that the enemy will try to keep between it and the goal
    public float maxZGoalOffset = .7f;     // Added by Abdul: The offset the enemy will go from the z

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

    public int blockFrames = 40;

    // Reference Variables
    private Rigidbody _rigidbody;
    private AttackBoundsManager _heavyBounds;
    private AttackBoundsManager _mediumBounds;
    private AttackBoundsManager _lightBounds;

    [SerializeField]
    public PlayerStateMachine _currentPlayerMachine;

    // State variables
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    /// Attacked Indicators
    private bool _isAttacked;
    private AttackType[] _recievedAttack = new AttackType[6];

    // Player Attack Damages
    private int[] _playerAttackDamages = new int[6];

    // Other
    public bool _knockedDown; //changed to public to help with animation controls
    private bool _isGrounded;
    private float _knockdownMeter;
    private float _stunTimer;
    private int _currentHealth;
    private GameObject _enemy;

    // Movement and Attack Info
    private bool _moving;
    private bool _attacking;
    private float _lastAttacked;
    private Transform _movingGoal;
    private Vector2 _movingGoalOffset;

    // Materials
    private Material _baseMaterial;
    private Material _heavyBoundsMat;
    private Material _mediumBoundsMat;
    private Material _lightBoundsMat;
    
    // Async Check
    private bool _finishedInitialization;

    //// Getters and Setters
    public Rigidbody Rigidbody => _rigidbody;
    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public PlayerStateMachine CurrentPlayerMachine { get => _currentPlayerMachine; set => _currentPlayerMachine = value; }
    public bool IsAttacked => _isAttacked;
    //public AttackType[] RecievedAttack => _recievedAttack;
    private Dictionary<GameObject, AttackType> _receivedAttacks = new Dictionary<GameObject, AttackType>();
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
    public GameObject Enemy { get => _enemy; }
    public Transform MovingGoal { get => _movingGoal; set => _movingGoal = value; }
    public Vector2 MovingGoalOffset { get => _movingGoalOffset; set => _movingGoalOffset = value; }
    public SpriteEffects SpriteEffects { get => gameObject.GetComponent<SpriteEffects>(); }
    public bool FinishedInitialization { get => _finishedInitialization; }

    // Added of 8/1/2023
    public bool DontAttack { get; set; }
    public bool CanPursue { get; set; }
    public FlankType EnemyFlankType { get; set; }
    public float EnemyFlankDistanceGoal{ get; set; } 
    public bool inPosition = false; //used for animation controller
    // public Vector3 guardPosition;
    public bool isBlocking = false;
    public GameObject healthpackPrefab;

    // Added 8/7/2023 and 8/9/2023 and 8/10/2023
    public Vector3 realMovingGoal { get; set; }
    public GameObject AgentObject { get; set; }
    public NavMeshAgent RealAgent { get; set; }
    public bool HasAgent{ get; set; }
    public bool ForceIdleAnim{ get; set; }
    public float MaxZGoalOffset { get => maxZGoalOffset; }


    // Functions    
    public void Initialize() {
        _currentPlayerMachine = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        
        // _recievedAttack[(int)Attacks.LightAttack1] = new AttackType("FirstLightAttack", new Vector2(10, 500), 40, 5);
        // _recievedAttack[(int)Attacks.LightAttack2] = new AttackType("SecondLightAttack", new Vector2(50, 250), 60, 15);
        // _recievedAttack[(int)Attacks.LightAttack3] = new AttackType("ThirdLightAttack", new Vector2(150, 500), 100, 30);
        // _recievedAttack[(int)Attacks.MediumAttack1] = new AttackType("FirstMediumAttack", new Vector2(50, 500), 70, 40);
        // _recievedAttack[(int)Attacks.MediumAttack2] = new AttackType("SecondMediumAttack", new Vector2(800, 100), 80, 50);
        // _recievedAttack[(int)Attacks.Slam] = new AttackType("SlamAttack", new Vector2(800, 800), 300, 5);

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
        _enemy = gameObject;

        // Materials
        _baseMaterial = body.GetComponent<Renderer>().material;
        _heavyBoundsMat = heavyAttackBounds.GetComponent<Renderer>().material;
        _mediumBoundsMat = mediumAttackBounds.GetComponent<Renderer>().material;
        _lightBoundsMat = lightAttackBounds.GetComponent<Renderer>().material;

        // Begins the initial state. All Awake code should go before here unless you want it defined after the initial
        // states EnterState()
        _currentState = _states.Idle();
        _currentState.EnterState();
        _finishedInitialization = true;

        // Determining Left/Right
        if(enemyType == EnemyType.Boss) {
            EnemyFlankType = FlankType.Boss;
        }else if (Mathf.Abs(leftEnemies - rightEnemies) < 3) {
            int dirNumber = UnityEngine.Random.Range(1, 3); // 1 for right, 2 for left
            if (dirNumber == 1) {
                EnemyFlankType = FlankType.Right;
                rightEnemies++;
            } else {
                EnemyFlankType = FlankType.Left;
                leftEnemies++;
            }
        } else {
            if (leftEnemies > rightEnemies) {
                EnemyFlankType = FlankType.Right;
                rightEnemies++;
            } else {
                EnemyFlankType = FlankType.Left;
                leftEnemies++;
            }
        }
        // EnemyFlankType = FlankType.Right;
        // rightEnemies++;

        CanPursue = false;
        ForceIdleAnim = false;
        // guardPosition = transform.position;
    }

    void Update() {
        _isGrounded = CheckIfGrounded();
        if (_currentPlayerMachine == null) return;
        if (CurrentState != null) {
            _currentState.UpdateStates();
        }

        // Debug.Log(CurrentState + " sub: " + CurrentState.CurrentSubState);
        // Debug.Log("IS GROUNDED?: " + IsGrounded);
    }

    private void FixedUpdate() {
        _isGrounded = CheckIfGrounded();
        if (_currentPlayerMachine == null) return;
        if (CurrentState != null) {
            _currentState.FixedUpdateStates();
        }
    }

    public bool CheckIfGrounded()
    {
        RaycastHit hit;
        Vector3 curPos = transform.position;
        Debug.DrawRay(curPos, -Vector3.up * 0.5f, Color.red);
        if (Physics.Raycast(new Vector3(curPos.x, curPos.y + 0.1f, curPos.z), -transform.up * .5f, out hit, .1f)) {
            if(hit.collider.CompareTag("Ground"))
                return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
        // be first before anything else
        //ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        AttackBoundsManager otherAttackManager;
        if (other.TryGetComponent<AttackBoundsManager>(out otherAttackManager) && !isBlocking) {
            if (_receivedAttacks.ContainsKey(other.gameObject)) return;
            
            AttackType receivedAttack = new AttackType(otherAttackManager.selectedAttack.ToString(),
                otherAttackManager.knockback, otherAttackManager.pressure, otherAttackManager.damage);
            
            if (other.transform.parent.position.x > transform.position.x) receivedAttack.AttackedFromRightSide = true;
            _receivedAttacks[other.gameObject] = receivedAttack;
            _isAttacked = true;
        }
    }

    // private void OnTriggerExit(Collider other) {
    //     // Important function for ensuring that the triggerExit works even if the other trigger is disabled. This must
    //     // be first before anything else
    //     ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
    //     //bool checkIfStillAttacked = false;
    //     if (_receivedAttacks.ContainsKey(other.gameObject)) {
    //         _receivedAttacks.Remove(other.gameObject);
    //     }
    //     // for (int i = 0; i < _recievedAttack.Length; i++) {
    //     //     if (other.CompareTag(_recievedAttack[i].Tag)) {
    //     //         _recievedAttack[i].Used = false;
    //     //         _recievedAttack[i].AttackedFromRightSide = false;
    //     //         _recievedAttack[i].StatsApplied = false;
    //     //     }
    //     //     if (_recievedAttack[i].Used) {
    //     //         checkIfStillAttacked = true;
    //     //     }
    //     // }
    //
    //     // _isAttacked = checkIfStillAttacked;
    //     //_isAttacked = false;
    // }

    private void OnDestroy() {
        GameManager.Instance.EnemyReferences.Remove(this);
    }

    public List<string> ApplyAttackStats() {
        List<string> recievedAttackNames = new List<string>();


        if(isBlocking){//when blocking ignore all damage
            _receivedAttacks.Clear();
            _isAttacked = false;
            return recievedAttackNames;
        }

        foreach (AttackType i in _receivedAttacks.Values) {
            if (_knockdownMeter > 0) {
                KnockdownMeter -= i.KnockdownPressure;
            }
            if (_knockdownMeter < 0) {
                _knockedDown = true;
            }
            if (KnockedDown) {
                Vector2 appliedKnockback = i.KnockbackDirection;
                if (i.AttackedFromRightSide) {
                    appliedKnockback = new Vector2(appliedKnockback.x * -1, appliedKnockback.y);
                }
                // appliedKnockback = new Vector2(appliedKnockback.x * 8, appliedKnockback.y);//elygh added this to increase knockback
                Rigidbody.velocity = Vector3.zero;
                // Debug.Log("Knockback Applied: " + appliedKnockback + " from " + i);
                Rigidbody.AddForce(new Vector3(appliedKnockback.x, appliedKnockback.y, 0));
                // Debug.Log("applied knockback: " + appliedKnockback.x + "     player x scale:" + transform.localScale.x);
            }
            CurrentHealth -= i.Damage;
            //Debug.Log("DAMAGE TO ENEMY: " + _recievedAttack[i].Damage + " HEALTH: " + currentHealth);
            recievedAttackNames.Add(i.Name);
        }
        _receivedAttacks.Clear();
        _isAttacked = false;
        return recievedAttackNames;
    }

    public void SetDead() {
        GameManager.Instance.EnemyReferences.Remove(this);
        _enemy.SetActive(false);

        int randomNumber = UnityEngine.Random.Range(1, 4);//33%
        if(randomNumber == 1){
            Vector3 new_position = transform.position;
            new_position = new Vector3(new_position.x, 0.6f, new_position.z);
            Instantiate(healthpackPrefab, new_position, Quaternion.identity);
        }
    }

    public bool CheckPath() {
        if (!HasAgent) { return true; }

        NavMeshPath navPath = new NavMeshPath();
        bool success = RealAgent.CalculatePath(CurrentPlayerMachine.transform.position, navPath);
        return !((!success) || (navPath.status != NavMeshPathStatus.PathComplete));
    }

    /// <summary>
    /// Calculates the speed of the enemy
    /// </summary>
    public void SpeedControl() {
        Vector3 enemyVelocity = Rigidbody.velocity;
        Vector3 flatVelocity = new Vector3(enemyVelocity.x, 0f, enemyVelocity.z);
      
        // limit velocity if needed
        if (flatVelocity.magnitude > movementSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z);
        }
    }
    public IEnumerator DeathTimeDelay(float waitTime){
        yield return new WaitForSeconds(waitTime);
        if(enemyType != EnemyType.Boss){
            this.SetDead();
        }else {
            SceneManager.LoadScene("Win_Screen");
        }
        
    }
    
    public GameObject InstantiatePrefab(GameObject obj) {
        return Instantiate(obj);
    }

    public void ClearRecievedAttacks() {
        _receivedAttacks.Clear();
        _isAttacked = false;
    }
}
