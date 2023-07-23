using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private PlayerStateMachine _playerRef = null;
    [SerializeField]
    private List<EnemyStateMachine> _enemyReferences = new List<EnemyStateMachine>();
    [SerializeField]
    private InputSystem _inputSystem = null;
    private PowerupSystem _powerupSystem = null;
    private bool FirstLoad = true;

    public PlayerStateMachine PlayerRef { get => _playerRef; set => _playerRef = value; }
    public List<EnemyStateMachine> EnemyReferences { get => _enemyReferences;}
    public InputSystem InputSystem { get => _inputSystem; set => _inputSystem = value; }
    public PowerupSystem PowerupSystem { get => _powerupSystem; set => _powerupSystem = value; }

    IEnumerator AddEnemies() {
        while (_playerRef == null) {
            Debug.Log("Awaiting player reference assignment before performing enemy reference assignment");
            yield return null;
        }
        _enemyReferences.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject i in enemies) {
            EnemyStateMachine newEnemy = i.GetComponent<EnemyStateMachine>();
            newEnemy.CurrentPlayerMachine = PlayerRef;
            newEnemy.Initialize();
            _enemyReferences.Add(newEnemy);
        }
    }

    IEnumerator AddPlayer() {
        while (_inputSystem == null) {
            yield return null;
        }
        _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        _playerRef.InputSystem = _inputSystem;
    }
    
    // Start is called before the first frame update
    void Awake() {
        CheckDuplicates();
        _inputSystem = GetComponent<InputSystem>();
        StartCoroutine(AddEnemies());
        StartCoroutine(AddPlayer());
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!FirstLoad) {
            StartCoroutine(AddEnemies());
            StartCoroutine(AddPlayer());
        }
        FirstLoad = false;
    }

    /// <summary>
    /// Checks for any duplicate GameManagers, if any are found, then this GameManager instance is not the original. 
    /// Deletes self if not the original
    /// </summary>
    public void CheckDuplicates() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
}
