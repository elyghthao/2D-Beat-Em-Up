using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async void AddEnemies() {
        while (_playerRef == null) {
            Debug.Log("Awaiting player reference assignment before performing enemy reference assignment");
            await Task.Yield();
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

    public async void AddPlayer() {
        while (_inputSystem == null) {
            await Task.Yield();
        }
        _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        _playerRef.InputSystem = _inputSystem;
    }
    
    // Start is called before the first frame update
    void Awake() {
        CheckDuplicates();
        AddEnemies();
        AddPlayer();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!FirstLoad) {
            AddEnemies();
        }
        FirstLoad = false;
    }

    /// <summary>
    /// Checks for any duplicate GameManagers, if any are found, then this GameManager instance is not the original. 
    /// Deletes self if not the original
    /// </summary>
    public void CheckDuplicates() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}
