using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StaminaBarController : MonoBehaviour {
   public GameObject staminaBar;
   public Color leftColor;
   public Color rightColor;
   //public EnemyStateMachine enemyState;
   public PlayerStateMachine playerState;
   public float boarderRound;
   public float staminaBarRound;
   public float currentStamina = 100;
   public float maxStamina = 150;
   public Vector3 offset = Vector3.zero;
   public Vector3 sizeOffset = Vector3.one;
   private Material _GUI_Material;
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
      if (staminaBar == null) {
         Debug.LogWarning("StaminaBar component not assigned on object: " + gameObject
                                                                         + ", child of, " + transform.parent);
         return;
      }

      // Swap the line below for the next line when using ExecuteInEditMode
      //_GUI_Material = staminaBar.GetComponent<Renderer>().sharedMaterial;
      _GUI_Material = staminaBar.GetComponent<Renderer>().material;
       if (/*enemyState == null && */playerState == null) {
          Debug.LogWarning("StaminaObject not attached to an object with a state machine,"
                           + " current parent: " + transform.parent);
          return;
       }
      
       _isPlayer = playerState != null;
       if (_isPlayer) {
          transform.position = playerState.transform.position + offset;
          maxStamina = playerState.stamina;
          currentStamina = playerState.Stamina;
          // return;
       }
      
       // transform.position = enemyState.transform.position + offset;
       // maxStamina = enemyState.maxHealth;
       // currentStamina = enemyState.CurrentHealth;
   }

   // Update is called once per frame
   private void Update() {
      if (!_initialized) {
         _initialized = true;
         _isPlayer = playerState != null;
         if (_isPlayer) maxStamina = playerState.stamina;
         // else if (enemyState != null)
         //    maxStamina = enemyState.maxHealth;
         else _initialized = false;
         transform.localScale = Vector3.Scale(transform.localScale, sizeOffset);
      }
      
      if (!CheckReferences()) {
         maxStamina = -1;
         currentStamina = -1;
         leftColor = Color.magenta;
         return;
      }
      // if (_isPlayer)
      PlayerUpdate();
      // else
      //    EnemyUpdate();

      _GUI_Material.SetColor(LeftColor, leftColor);
      _GUI_Material.SetColor(RightColor, rightColor);
      _GUI_Material.SetFloat(MaxValue, maxStamina);
      _GUI_Material.SetFloat(CurrentValue, currentStamina);
      _GUI_Material.SetFloat(BoarderRound, boarderRound);
      _GUI_Material.SetFloat(BarRound, staminaBarRound);
   }

   private void PlayerUpdate() {
      transform.position = playerState.transform.position + offset;
      currentStamina = playerState.Stamina;
      if (!playerState.gameObject.activeSelf) gameObject.SetActive(false);
   }

   // private void EnemyUpdate() {
   //    transform.position = enemyState.transform.position + offset;
   //    currentStamina = enemyState.CurrentHealth;
   //    if (!enemyState.gameObject.activeSelf) gameObject.SetActive(false);
   // }

   private bool CheckReferences() {
      if (playerState == null/* && enemyState == null*/) return false;
      if (_GUI_Material == null) return false;
      return staminaBar != null;
   }
}
