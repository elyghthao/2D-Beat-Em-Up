using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private PlayerStateMachine _playerRef = null;
    [SerializeField]
    private List<EnemyStateMachine> _enemyReferences = new List<EnemyStateMachine>();
    [SerializeField]
    private InputSystem _inputSystem = null;
    private PowerupSystem _powerupSystem = null;

    public PlayerStateMachine PlayerRef { get => _playerRef; set => _playerRef = value; }
    public List<EnemyStateMachine> EnemyReferences { get => _enemyReferences;}
    public InputSystem InputSystem { get => _inputSystem; set => _inputSystem = value; }
    public PowerupSystem PowerupSystem { get => _powerupSystem; set => _powerupSystem = value; }

    public void AddEnemy(EnemyStateMachine newEnemy) {
        newEnemy.CurrentPlayerMachine = PlayerRef;
        _enemyReferences.Add(newEnemy);
    }

    public void AddPlayer(PlayerStateMachine newPlayer) {
        _playerRef = newPlayer;
        foreach (EnemyStateMachine i in _enemyReferences) {
            i.CurrentPlayerMachine = _playerRef;
        }
    }
    
    // Start is called before the first frame update
    void Awake() {
        _inputSystem = GetComponent<InputSystem>();
        _inputSystem.Initialize();
        _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
        _playerRef.Initialize();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject i in enemies) {
            EnemyStateMachine enemyRef = i.GetComponent<EnemyStateMachine>();
            _enemyReferences.Add(enemyRef);
            enemyRef.Initialize();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("New scene loaded");
        GameObject[] duplicates = GameObject.FindGameObjectsWithTag("GameController");
        foreach (GameObject i in duplicates) {
            if(i != gameObject) {
                Destroy(i);
            }
        }
    }
}
