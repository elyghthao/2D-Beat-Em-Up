using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Types of comperisons.
/// </summary>
public enum ComparisonType
{
    Equals = 1,
    NotEqual = 2,
    GreaterThan = 3,
    SmallerThan = 4,
    SmallerOrEqual = 5,
    GreaterOrEqual = 6
}

public class Attack : MonoBehaviour {
    
    public enum AttackName {LightAttack, LightAttack1, LightAttack2, MediumAttack, MediumAttack1, SlamAttack };
    public AttackName selectedAttack;
    public string Tag { get; private set; }
    public Vector2 Knockback { get; private set; }
    public float Pressure { get; private set; }
    public float Damage { get; private set; }

    public void Awake() {
        Knockback = new Vector2(0, 0);
        Pressure = 0;
        Damage = 0;
        tag = "";
    }

    private void Update() {
        if (Tag.Length > 0) return;
        switch (selectedAttack) {
            case AttackName.LightAttack:
                Tag = "FirstLightAttack";
                break;
            case AttackName.LightAttack1:
                Tag = "SecondLightAttack";
                break;
            case AttackName.LightAttack2:
                Tag = "ThirdLightAttack";
                break;
            case AttackName.MediumAttack:
                Tag = "FirstMediumAttack";
                break;
            case AttackName.MediumAttack1:
                Tag = "SecondMediumAttack";
                break;
            case AttackName.SlamAttack:
                Tag = "SlamAttack";
                break;
            default:
                Tag = "";
                break;
        }
    }
}
