using UnityEngine;

/// <summary>
///    Attack Type struct for identifying when we've been hit by a specific attack
/// </summary>
public class AttackType {
   public AttackType(string name, Vector2 knockbackDirection, float knockdownPressure, int damage) {
      Name = name;
      KnockbackDirection = knockbackDirection;
      KnockdownPressure = knockdownPressure;
      Damage = damage;
   }

   public string Name { get; }

   // Whether this attack has collided with us or not
   public float KnockdownPressure { get; }

   public int Damage { get; }

   public Vector2 KnockbackDirection { get; }

   public bool Used { get; set; }

   public bool AttackedFromRightSide { get; set; }
}