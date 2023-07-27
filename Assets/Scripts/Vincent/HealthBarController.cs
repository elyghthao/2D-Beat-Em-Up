using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class HealthBarController : MonoBehaviour {
    public GameObject healthBar;
    public Color leftColor;
    public Color rightColor;
    private Material _healthMaterial;
    private EnemyStateMachine _enemyState;
    private PlayerStateMachine _playerState;
    public float _currentHealth = 100;
    public float _maxHealth = 150;
    private bool _isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (healthBar == null) {
            Debug.LogWarning("HealthBar component not assigned on object: " + gameObject
                                                + ", child of, " + transform.parent);
            return;
        }

        _healthMaterial = healthBar.GetComponent<Renderer>().material;
        _enemyState = transform.parent.gameObject.GetComponent<EnemyStateMachine>();
        _playerState = transform.parent.gameObject.GetComponent<PlayerStateMachine>();
        if (_enemyState == null && _playerState == null) {
            Debug.LogWarning("HealthObject not attached to an object with a state machine,"
                                                        + " current parent: " + transform.parent);
            return;
        }
        _isPlayer = _playerState != null;
        if (_isPlayer) {
            _maxHealth = _playerState.maxHealth;
            _currentHealth = _playerState.CurrentHealth;
            return;
        }

        _maxHealth = _enemyState.maxHealth;
        _currentHealth = _enemyState.CurrentHealth;
    }

    // Update is called once per frame
    private void Update() {
        //if (_healthMaterial == null || (_playerState == null && _enemyState == null)) return;
        if (_isPlayer) {
            _currentHealth = _playerState.CurrentHealth;
        } else {
            _currentHealth = _enemyState.CurrentHealth;
            Debug.Log("EnemyHealth: " + _currentHealth);
        }
        _healthMaterial.SetColor("_LeftColor", leftColor);
        _healthMaterial.SetColor("_RightColor", rightColor);
        _healthMaterial.SetFloat("_MaxHealth", _maxHealth);
        _healthMaterial.SetFloat("_CurrentHealth", _currentHealth);
    }
}
