using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pack : MonoBehaviour
{
    private AudioSource health_pack_collect_sound; // The sound clip for when the health pack is picked up.
    private BoxCollider health_pack_collider; // The health pack's collider.
    private MeshRenderer health_pack_mesh; // The health pack's mesh renderer.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the health pack's audio source, box collider, and mesh renderer
        // components are given to the associated variables for access.
        health_pack_collect_sound = GetComponent<AudioSource>();
        health_pack_collider = GetComponent<BoxCollider>();
        health_pack_mesh = GetComponent<MeshRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // If the health pack comes into contact with the player's collision,
        // signified by the tag "Player"...
        if (other.gameObject.CompareTag("Player"))
        {
            // ...then the health pack pickup sound effect plays, and the
            // health pack's collider and mesh are both disabled.
            health_pack_collect_sound.Play();
            health_pack_collider.enabled = false;
            health_pack_mesh.enabled = false;

            // Finally, the health pack object is destroyed.
            Destroy(gameObject, 1);
        }
    }
}
