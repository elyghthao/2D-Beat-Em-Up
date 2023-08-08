using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
   public static GameManager Instance { get; private set; }
   public static GameObject SmackedPrefabInstance { get; private set; }
   public PowerupSystem PowerupSystem { get; set; }

   public GameObject smackedVFXPrefab;
   public GameObject healthBarPrefab;
   public GameObject staminaBarPrefab;
   private bool FirstLoad = true;

   public AudioListener audio;
   public GameObject audioSlider;
   public float volume = 0.1f;

   [SerializeField] private PlayerStateMachine _playerRef;
   [SerializeField] private List<EnemyStateMachine> _enemyReferences;
   [SerializeField] private InputSystem _inputSystem;

   public PlayerStateMachine PlayerRef {
      get => _playerRef;
      set => _playerRef = value;
   }

   public List<EnemyStateMachine> EnemyReferences => _enemyReferences;
   
    // Update is called once per frame
    void Update() {
       if (audioSlider != null) {
          volume = audioSlider.GetComponent<Slider>().value;
          AudioListener.volume = audioSlider.GetComponent<Slider>().value;
       }
       Cheats();
    }

    public void Cheats() {
       if (Input.GetKey(KeyCode.Alpha1)) {
          SceneManager.LoadScene("Scenes/MainScenes/Main_Menu");
          return;
       }

       if (Input.GetKey(KeyCode.Alpha2)) {
          SceneManager.LoadScene("Scenes/MainScenes/Level_1");
          return;
       }
       if (Input.GetKey(KeyCode.Alpha3)) {
          SceneManager.LoadScene("Scenes/MainScenes/Level_2");
          return;
       }
       if (Input.GetKey(KeyCode.Alpha4)) {
          SceneManager.LoadScene("Scenes/MainScenes/Level_3");
          return;
       }
       if (Input.GetKey(KeyCode.Alpha5)) {
          SceneManager.LoadScene("Scenes/MainScenes/You_Win_Screen");
          return;
       }
       if (Input.GetKey(KeyCode.Equals)) {
          _playerRef.CurrentHealth = _playerRef.maxHealth;
          return;
       }
       if (Input.GetKey(KeyCode.Alpha0)) {
          _playerRef.CurrentHealth = 10000000;
       }
       if (Input.GetKey(KeyCode.Alpha6)) {
          _playerRef.Stamina = 1000000000;
       }
       if (Input.GetKey(KeyCode.Alpha7)) {
          _playerRef.Stamina = _playerRef.stamina;
       }
    }
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

      SmackedPrefabInstance = smackedVFXPrefab;

      Application.targetFrameRate = 60;

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

      audio = GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>();
      audioSlider = GameObject.FindWithTag("VolumeController");
      if(SceneManager.GetActiveScene().name.ToString() != "Main_Menu" && audioSlider != null){
         audioSlider.SetActive(false);
      }
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
         healthBarController.sizeOffset = new Vector3(1f, 1f, 1f);
      }
   }

   private void AddPlayer() {
      try{
         _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
         _playerRef.InputSys = _inputSystem;
         
         // Instantiate healthbar for player
         GameObject currentHealthBar = Instantiate(healthBarPrefab);
         var healthBarController = currentHealthBar.GetComponent<HealthBarController>();
         healthBarController.playerState = _playerRef;
         healthBarController.offset = new Vector3(0, 5, 0);
         healthBarController.sizeOffset = new Vector3(1f, 1f, 1f);
         healthBarController.leftColor = Color.red;

         GameObject currentStaminaBar = Instantiate(staminaBarPrefab);
         var staminaBarController = currentStaminaBar.GetComponent<StaminaBarController>();
         staminaBarController.playerState = _playerRef;
         staminaBarController.offset = new Vector3(0, 5.25f, 0);
         staminaBarController.sizeOffset = new Vector3(1f, 1f, 1f);
         staminaBarController.leftColor = Color.green;
      } catch (Exception e){ Debug.Log("Player couldn't be added to GameManager:\n" + e);}
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
         audio = GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>();
         try {
            audioSlider = GameObject.FindWithTag("VolumeController");
            audioSlider.GetComponent<Slider>().value = volume;
            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name.ToString() != "Main_Menu") {
               audioSlider.SetActive(false);
            }
         } catch (Exception) {}
      }
      FirstLoad = false;
   }
}
