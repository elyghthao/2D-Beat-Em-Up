using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance { get; private set; }

    public GameObject healthBarPrefab;

    [SerializeField]
    private PlayerStateMachine _playerRef;
    [SerializeField]
    private List<EnemyStateMachine> _enemyReferences;
    [SerializeField]
    private InputSystem _inputSystem;
    private PowerupSystem _powerupSystem;
    private bool FirstLoad = true;

    public PlayerStateMachine PlayerRef { get => _playerRef; set => _playerRef = value; }
    public List<EnemyStateMachine> EnemyReferences { get => _enemyReferences;}
    public InputSystem InputSystem { get => _inputSystem; set => _inputSystem = value; }
    public PowerupSystem PowerupSystem { get => _powerupSystem; set => _powerupSystem = value; }

    private void AddEnemies() {
        //while (_playerRef == null) {
        //    //Debug.Log("Awaiting player reference assignment before performing enemy reference assignment");
        //    yield return null;
        //}
        _enemyReferences.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject i in enemies) {
            if (i.GetComponent<EnemyStateMachine>() == null) continue;
            EnemyStateMachine newEnemy = i.GetComponent<EnemyStateMachine>();
            newEnemy.CurrentPlayerMachine = PlayerRef;
            newEnemy.Initialize();
            _enemyReferences.Add(newEnemy);
            GameObject currentHealthBar = Instantiate(healthBarPrefab);
            HealthBarController healthBarController = currentHealthBar.GetComponent<HealthBarController>();
            healthBarController.enemyState = newEnemy;
            healthBarController.offset = new Vector3(0, 5, 0);
            healthBarController.sizeOffset = new Vector3(1.5f, 1.5f, 1f);
        }
    }

    private void AddPlayer() {
        _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        _playerRef.InputSys = _inputSystem;
        GameObject currentHealthBar = Instantiate(healthBarPrefab);
        HealthBarController healthBarController = currentHealthBar.GetComponent<HealthBarController>();
        healthBarController.playerState = _playerRef;
        healthBarController.offset = new Vector3(0, 5, 0);
        healthBarController.sizeOffset = new Vector3(1.5f, 1.5f, 1f);
        healthBarController.leftColor = Color.green;
    }
    
    // Start is called before the first frame update
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _inputSystem = GetComponent<InputSystem>();
        AddPlayer();
        if (PlayerRef == null) {
            Debug.LogWarning("Player was not assigned, might be in a scene without a player prefab...");
        } else {
            AddEnemies();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!FirstLoad) {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            AddPlayer();
            if (_playerRef == null) {
                Debug.LogWarning("Player was not assigned, might be in a scene without a player prefab...");
            } else {
                AddEnemies();
            }
        }
        FirstLoad = false;
    }
}
