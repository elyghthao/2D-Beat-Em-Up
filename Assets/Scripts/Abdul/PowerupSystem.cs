using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSystem : MonoBehaviour {
    // ============================================ PUBLIC VARIABLES ============================================
    // Power-up enumerations that represent the different types of power-ups
    public enum Powerup {
        None,
        Dash,
        Slam,
        PowerBlock,
    };

    // ============================================ PRIVATE VARIABLES ============================================
    private Powerup _equippedPowerup = Powerup.Slam; // Currently equipped powerup
    private Dictionary<Powerup, bool> _unlockedPowerups = new Dictionary<Powerup, bool>(); // The dictionary determining which powerups are unlocked

    // ============================================ PRIVATE METHODS/FUNCTIONS ============================================
    // Start is called before the first frame update
    void Start() {
        // Initiliazing power-ups to be unlocked
        _unlockedPowerups[Powerup.None] = true;
        _unlockedPowerups[Powerup.Dash] = false;
        _unlockedPowerups[Powerup.Slam] = false;
        _unlockedPowerups[Powerup.PowerBlock] = false;
    }

    // ============================================ PUBLIC METHODS/FUNCTIONS ============================================
    /*
    To make the PowerupSystem persistent, adding it to the GameManager
    */
    private void Awake() {
        GetComponent<GameManager>().PowerupSystem = this;
    }
    
    /*
    Unlocks the given power-up.
    */
    public void unlockPowerup(Powerup unlockedPowerup) { _unlockedPowerups[unlockedPowerup] = true; }

    /*
    Returns if the given power-up is unlocked or not
    */
    public bool checkPowerup(Powerup checkingPowerup) { return _unlockedPowerups[checkingPowerup]; }

    /*
    Returns the currently equipped power-up.
    */
    public Powerup getEquipped() { return _equippedPowerup; }

    /*
    Equips the given power-up.
    */
    public void equipPowerup(Powerup toEquip) { _equippedPowerup = toEquip; }

    /*
    Checks if the given Powerup is equipped or not
    */
    public bool isEquipped(Powerup check) { return _equippedPowerup == check; }

    /*
    Checks if an attack=type powerup is equipped
    */
    public bool attackEquipped() { 
        return _equippedPowerup == Powerup.Dash || _equippedPowerup == Powerup.Slam; 
    }
}
