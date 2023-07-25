// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Used for the VFX such as the slam smoke effect
// /*
// HOW TO USE:
// Setup the effects in the actual character, then disable it.
// After that, add an EffectObject using the inspector and fill out the information.
// Lastly, just call the 'doEffect' method with the name of the effect and done!
// */
// public class SpriteEffects : MonoBehaviour {
//     // ============================================ PUBLIC VARIABLES ============================================
//     [System.Serializable]
//     public class EffectObject {
//         public string name; // Effect name
//         public GameObject effect; // Actual effect on the player
//         public float length; // The length the effect lasts for
//         public bool attach; // If true, attaches the effect to the player
//     }

//     public EffectObject[] objects; // Objects

//     // ============================================ PRIVATE VARIABLES ============================================
//     private Dictionary<string, EffectObject> _effects = new Dictionary<string, EffectObject>(); // Stores information about each effect
//     private static Transform _effectsParent; // Used for effects that are not attached


//     // ============================================ PRIVATE METHODS ============================================
//     // Used to loop through the array and put each element in the dictionary
//     void Awake() {
//         for (int i = 0; i < objects.Length; i++) {
//             EffectObject obj = objects[i];
//             _effects.Add(obj.name, obj);
//         }

//         if (_effectsParent == null) {
//             GameObject newObj = new GameObject("_EFFECT");
//             _effectsParent = newObj.transform;
//         }
//     }

//     // ============================================ PUBLIC METHODS ============================================
//     public void doEffect(string name, bool flip) {
//         // Finding the effect
//         EffectObject obj = _effects[name];
//         if (obj == null) {
//             Debug.LogWarning("SpriteEffects: Given name '" + name + "' does not match any effect.");
//             return;
//         }  

//         // Getting correct parent
//         Transform objParent = _effectsParent;
//         Vector3 pos = obj.effect.transform.position;
//         if (obj.attach) {
//             objParent = transform;
//         }

//         // Creating effect
//         GameObject newEffect = Instantiate(obj.effect, objParent);
//         newEffect.transform.position = pos;
//         newEffect.SetActive(true);
//         Destroy(newEffect, obj.length);

//         // Checking flip
//         if (flip) {
//             newEffect.GetComponent<SpriteRenderer>().flipX = true;
//         }
//     }

//     public void doEffect(string name) { doEffect(name, false); }
// }
