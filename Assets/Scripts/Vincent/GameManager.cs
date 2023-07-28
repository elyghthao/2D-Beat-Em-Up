using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
   
   public static GameManager Instance { get; private set; }
   public PowerupSystem PowerupSystem { get; set; }
   
   public GameObject healthBarPrefab;
   private bool FirstLoad = true;

   [SerializeField] private PlayerStateMachine _playerRef;
   [SerializeField] private List<EnemyStateMachine> _enemyReferences;
   [SerializeField] private InputSystem _inputSystem;

   public PlayerStateMachine PlayerRef {
      get => _playerRef;
      set => _playerRef = value;
   }

   public List<EnemyStateMachine> EnemyReferences => _enemyReferences;

   public InputSystem InputSystem {
      get => _inputSystem;
      set => _inputSystem = value;
   }

   // Start is called before the first frame update
   private void Awake() {
      if (Instance != null && Instance != this) {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      _inputSystem = GetComponent<InputSystem>();
      AddPlayer();
      if (PlayerRef == null)
         Debug.LogWarning("Player was not assigned, might be in a scene without a player prefab...");
      else
         AddEnemies();
      SceneManager.sceneLoaded -= OnSceneLoaded;
      DontDestroyOnLoad(gameObject);
      SceneManager.sceneLoaded += OnSceneLoaded;
   }

   private void AddEnemies() {
      _enemyReferences.Clear();
      var enemies = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (var i in enemies) {
         if (i.GetComponent<EnemyStateMachine>() == null) continue;
         var newEnemy = i.GetComponent<EnemyStateMachine>();
         newEnemy.CurrentPlayerMachine = PlayerRef;
         newEnemy.Initialize();
         _enemyReferences.Add(newEnemy);
         
         // Instantiate healthbar for enemy
         var currentHealthBar = Instantiate(healthBarPrefab);
         var healthBarController = currentHealthBar.GetComponent<HealthBarController>();
         healthBarController.enemyState = newEnemy;
         healthBarController.offset = new Vector3(0, 5, 0);
         healthBarController.sizeOffset = new Vector3(1.5f, 1.5f, 1f);
      }
   }

   private void AddPlayer() {
      _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
      _playerRef.InputSys = _inputSystem;
      
      // Instantiate healthbar for player
      var currentHealthBar = Instantiate(healthBarPrefab);
      var healthBarController = currentHealthBar.GetComponent<HealthBarController>();
      healthBarController.playerState = _playerRef;
      healthBarController.offset = new Vector3(0, 5, 0);
      healthBarController.sizeOffset = new Vector3(1.5f, 1.5f, 1f);
      healthBarController.leftColor = Color.green;
   }

   private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (!FirstLoad) {
         if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
         }

         Instance = this;
         AddPlayer();
         if (_playerRef == null)
            Debug.LogWarning("Player was not assigned, might be in a scene without a player prefab...");
         else
            AddEnemies();
      }

      FirstLoad = false;
   }
}