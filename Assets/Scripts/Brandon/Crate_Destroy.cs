using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_Destroy : MonoBehaviour
{
    private AudioSource crate_break_sound; // The sound clip for when the crate is destroyed.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the crate's audio source and mesh renderer components are
        // given to the variable for access.
        crate_break_sound = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // If the crate comes into contact with the player's attack hitboxes,
        // signified by the tag "Attack Hitbox", the crate stops being rendered
        // and the crate breaking sound effect plays.
        if (other.gameObject.CompareTag("Attack Hitbox"))
        {
            crate_break_sound.Play();
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 1);
        }
    }
}
