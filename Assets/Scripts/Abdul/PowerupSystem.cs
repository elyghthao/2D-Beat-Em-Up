using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSystem : MonoBehaviour {
    // ============================================ PUBLIC VARIABLES ============================================
    // Power-up enumerations that represent the different types of power-ups
    public enum Powerup {
        None,
        Dash,
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
    }

    // Update is called once per frame
    void Update() {
        
    }


    // ============================================ PUBLIC METHODS/FUNCTIONS ============================================
    /*
    Unlocks the given powerup
    */
    public void unlockPowerup(Powerup unlockedPowerup) {
        _unlockedPowerups[unlockedPowerup] = true;
    }

    /*
    Returns if the given powerup is unlocked or not
    */
    public bool checkPowerup(Powerup checkingPowerup) {
        return _unlockedPowerups[checkingPowerup];
    }
}
