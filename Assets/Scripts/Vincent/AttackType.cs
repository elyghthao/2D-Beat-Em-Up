using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack Type struct for identifying when we've been hit by a specific attack
/// </summary>
public class AttackType
{
    public AttackType(string tag, Vector2 knockbackDirection, float knockdownPressure, int damage) {
        this.tag = tag;
        this.knockbackDirection = knockbackDirection;
        this.knockdownPressure = knockdownPressure;
        this.damage = damage;
    }
    
    private string tag;
    private float knockdownPressure;
    private int damage;
    private Vector2 knockbackDirection;
    private bool statsApplied;
    private bool used;
    private bool attackedFromRightSide;
    
    // Tag that we'll compare to the triggers tag
    public string Tag { get => tag; }
    // Whether this attack has collided with us or not
    public float KnockdownPressure { get => knockdownPressure; }
    public int Damage { get => damage; }
    public Vector2 KnockbackDirection { get => knockbackDirection; }
    
    public bool StatsApplied { get => statsApplied; set => statsApplied = value; }
    public bool Used { get => used; set => used = value; }
    public bool AttackedFromRightSide { get => attackedFromRightSide; set => attackedFromRightSide = value; }
    
}
