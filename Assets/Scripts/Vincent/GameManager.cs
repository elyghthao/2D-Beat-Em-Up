using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

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
    public float volume = 1;
    public AudioListener audio;//make this private later
    public GameObject audioSlider;

    public PlayerStateMachine PlayerRef { get => _playerRef; set => _playerRef = value; }
    public List<EnemyStateMachine> EnemyReferences { get => _enemyReferences;}
    public InputSystem InputSystem { get => _inputSystem; set => _inputSystem = value; }
    public PowerupSystem PowerupSystem { get => _powerupSystem; set => _powerupSystem = value; }

    IEnumerator AddEnemies() {
        while (_playerRef == null) {
            //Debug.Log("Awaiting player reference assignment before performing enemy reference assignment");
            yield return null;
        }
        _enemyReferences.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject i in enemies) {
            if (i.GetComponent<EnemyStateMachine>() == null) continue;
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
        try {
            _playerRef = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
            _playerRef.InputSys = _inputSystem;
        }catch (Exception){}
        
    }
    
    // Start is called before the first frame update
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        _inputSystem = GetComponent<InputSystem>();
        StartCoroutine(AddEnemies());
        StartCoroutine(AddPlayer());
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        audio = GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>();
        audioSlider = GameObject.FindWithTag("VolumeController");
        
    }

    // Update is called once per frame
    void Update() {
        volume = audioSlider.GetComponent<Slider>().value;
        AudioListener.volume = audioSlider.GetComponent<Slider>().value;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!FirstLoad) {
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            StartCoroutine(AddEnemies());
            StartCoroutine(AddPlayer());
        }
        FirstLoad = false;


        audio = GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>();
        audioSlider = GameObject.FindWithTag("VolumeController");
        audioSlider.GetComponent<Slider>().value = volume;
        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name.ToString() != "Main_Menu"){
            audioSlider.SetActive(false);
        }
        
    }

    /// <summary>
    /// Checks for any duplicate GameManagers, if any are found, then this GameManager instance is not the original. 
    /// Deletes self if not the original
    /// </summary>
    public void CheckDuplicates() {
        
    }
}
