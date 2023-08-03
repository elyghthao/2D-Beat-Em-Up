using UnityEngine;
using UnityEngine.Serialization;

//[ExecuteInEditMode]
public class HealthBarController : MonoBehaviour {
   public GameObject healthBar;
   public Color leftColor;
   public Color rightColor;
   public EnemyStateMachine enemyState;
   public PlayerStateMachine playerState;
   public float boarderRound;
   public float healthBarRound;
   public float currentHealth = 100;
   public float maxHealth = 150;
   public Vector3 offset = Vector3.zero;
   public Vector3 sizeOffset = Vector3.one;
   private Material _healthMaterial;
   private bool _initialized;
   private bool _isPlayer;
   
   // Cached property references
   private static readonly int LeftColor = Shader.PropertyToID("_LeftColor");
   private static readonly int RightColor = Shader.PropertyToID("_RightColor");
   private static readonly int MaxValue = Shader.PropertyToID("_MaxValue");
   private static readonly int CurrentValue = Shader.PropertyToID("_CurrentValue");
   private static readonly int BoarderRound = Shader.PropertyToID("_BoarderRound");
   private static readonly int BarRound = Shader.PropertyToID("_BarRound");

   // Start is called before the first frame update
   private void Start() {
      if (healthBar == null) {
         Debug.LogWarning("HealthBar component not assigned on object: " + gameObject
                                                                         + ", child of, " + transform.parent);
         return;
      }

      // Swap the line below for the next line when using ExecuteInEditMode
      //_healthMaterial = healthBar.GetComponent<Renderer>().sharedMaterial;
      _healthMaterial = healthBar.GetComponent<Renderer>().material;
       if (enemyState == null && playerState == null) {
          Debug.LogWarning("HealthObject not attached to an object with a state machine,"
                           + " current parent: " + transform.parent);
          return;
       }
      
       _isPlayer = playerState != null;
       if (_isPlayer) {
          transform.position = playerState.transform.position + offset;
          maxHealth = playerState.maxHealth;
          currentHealth = playerState.CurrentHealth;
          return;
       }
      
       transform.position = enemyState.transform.position + offset;
       maxHealth = enemyState.maxHealth;
       currentHealth = enemyState.CurrentHealth;
   }

   // Update is called once per frame
   private void Update() {
      if (!_initialized) {
         _initialized = true;
         _isPlayer = playerState != null;
         if (_isPlayer)
            maxHealth = playerState.maxHealth;
         else if (enemyState != null)
            maxHealth = enemyState.maxHealth;
         else
            _initialized = false;
         transform.localScale = Vector3.Scale(transform.localScale, sizeOffset);
      }
      
      if (!CheckReferences()) {
         maxHealth = -1;
         currentHealth = -1;
         leftColor = Color.magenta;
      }
      else {
         if (_isPlayer)
            PlayerUpdate();
         else
            EnemyUpdate();
      }

      _healthMaterial.SetColor(LeftColor, leftColor);
      _healthMaterial.SetColor(RightColor, rightColor);
      _healthMaterial.SetFloat(MaxValue, maxHealth);
      _healthMaterial.SetFloat(CurrentValue, currentHealth);
      _healthMaterial.SetFloat(BoarderRound, boarderRound);
      _healthMaterial.SetFloat(BarRound, healthBarRound);
   }

   private void PlayerUpdate() {
      transform.position = playerState.transform.position + offset;
      currentHealth = playerState.CurrentHealth;
      if (!playerState.gameObject.activeSelf) gameObject.SetActive(false);
   }

   private void EnemyUpdate() {
      transform.position = enemyState.transform.position + offset;
      currentHealth = enemyState.CurrentHealth;
      if (!enemyState.gameObject.activeSelf) gameObject.SetActive(false);
   }

   private bool CheckReferences() {
      if (playerState == null && enemyState == null) return false;
      if (_healthMaterial == null) return false;
      return healthBar != null;
   }
}