using UnityEngine;

/// <summary>
/// Attack Type struct for identifying when we've been hit by a specific attack
/// </summary>
public class AttackType
{
    public AttackType(string tag, Vector2 knockbackDirection, float knockdownPressure, int damage) {
        this._tag = tag;
        this._knockbackDirection = knockbackDirection;
        this._knockdownPressure = knockdownPressure;
        this._damage = damage;
    }
    
    private string _tag;
    private float _knockdownPressure;
    private int _damage;
    private Vector2 _knockbackDirection;
    private bool _statsApplied;
    private bool _used;
    private bool _attackedFromRightSide;
    
    // Tag that we'll compare to the triggers _tag
    public string Tag { get => _tag; }
    // Whether this attack has collided with us or not
    public float KnockdownPressure { get => _knockdownPressure; }
    public int Damage { get => _damage; }
    public Vector2 KnockbackDirection { get => _knockbackDirection; }
    
    public bool StatsApplied { get => _statsApplied; set => _statsApplied = value; }
    public bool Used { get => _used; set => _used = value; }
    public bool AttackedFromRightSide { get => _attackedFromRightSide; set => _attackedFromRightSide = value; }
    
}
