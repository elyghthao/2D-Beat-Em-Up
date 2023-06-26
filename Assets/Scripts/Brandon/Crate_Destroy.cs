using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_Destroy : MonoBehaviour
{
    AudioSource crate_break_sound; // The sound clip for when the crate is destroyed.
    MeshRenderer crate_mesh_rend; // The mesh rendering of the crate.

    bool crate_destroyed; // Tracks whether the crate is destroyed or not.

    // Start is called before the first frame update
    void Start()
    {
        crate_break_sound = GetComponent<AudioSource>();
        crate_mesh_rend = GetComponent<MeshRenderer>();
        crate_destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the crate is no longer being rendered and crate_destroyed is set to false...
        if (crate_mesh_rend.enabled == false && crate_destroyed == false)
        {
            // ...then play the crate destroyed sound clip and set crate_destroyed to true.
            crate_break_sound.Play();
            crate_destroyed = true;
        }
    }
}
