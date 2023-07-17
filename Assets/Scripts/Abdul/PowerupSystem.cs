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
    };

    // ============================================ PRIVATE VARIABLES ============================================
    private Powerup _equippedPowerup = Powerup.None; // Currently equipped powerup
    private Dictionary<Powerup, bool> _unlockedPowerups = new Dictionary<Powerup, bool>(); // The dictionary determining which powerups are unlocked

    // ============================================ PRIVATE METHODS/FUNCTIONS ============================================
    // Start is called before the first frame update
    void Start() {
        // Initiliazing power-ups to be unlocked
        _unlockedPowerups[Powerup.None] = true;
        _unlockedPowerups[Powerup.Dash] = false;
        _unlockedPowerups[Powerup.Slam] = false;
    }

    // ============================================ PUBLIC METHODS/FUNCTIONS ============================================
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
    public void equipPowerup(Powerup toEquip) { 
        _equippedPowerup = toEquip;

        // IDEA: INSTEAD OF THE STATE MACHINE CHECKING, COULD MAKE IT SO THAT THIS SYSTEM HERE
        // FLICKS THE BOOLEANS FOR THE STATE MACHINE FOR IT. OTHERWISE, COULD JUST USE THE ABOVE
        // METHOD SO EITHER WAY IT WILL WORK.
    }
}
