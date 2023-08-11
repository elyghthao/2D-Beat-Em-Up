using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AttackBoundsManager : MonoBehaviour {
   public enum AttackName {LightAttack, LightAttack1, LightAttack2, MediumAttack, MediumAttack1, SlamAttack };
   public AttackName selectedAttack;
   public Vector2 knockback = Vector2.zero;
   public float pressure = 0;
   public int damage = 0;
   public float staminaDrain;
   
   private BoxCollider _collider;
   private Material _material;

   private void Awake() {
      _collider = GetComponent<BoxCollider>();
      _material = GetComponent<Renderer>().material;
   }

   public void SetMatColor(Color color) {
      _material.color = color;
   }

   public Color GetMatColor() {
      return _material.color;
   }

   public void SetColliderActive(bool active) {
      if (_collider == null) {
         Debug.LogWarning("Collider was null, check to make sure you're calling AttackBoundsManager correctly");
         _collider = GetComponent<BoxCollider>();
         return;
      }
      _collider.enabled = active;
   }
}