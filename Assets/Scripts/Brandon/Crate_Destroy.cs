using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_Destroy : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the crate.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource crate_break_sound; // The sound clip for when the crate is destroyed.
    private BoxCollider crate_collider; // The crate's box collider.
    public MeshRenderer crate_mesh; // The crate's mesh renderer.
    public bool health_appear_guarantee; // Whether it is guaranteed that a health pack will spawn from the crate.
    public GameObject health_pack; // The health pack prefab.
    private GameObject fake_collider; // Fake collider inside the crate

    // Start is called before the first frame update
    void Start()
    {
        // On start, the crate's audio source and box collider
        // components are given to the associated variables for access.
        crate_break_sound = GetComponent<AudioSource>();
        crate_collider = GetComponent<BoxCollider>();
        fake_collider = gameObject.transform.Find("Collider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Every attack hitbox tag is used to check collision between the crate and
        // the player's attack hitboxes.
        foreach (string tag in tags_to_check)
        {
            // If the crate comes into contact with the player's attack hitbox,
            // signified by the currently compared tag...
            if (other.gameObject.CompareTag(tag))
            {
                // ...then the crate breaking sound effect plays, and the crate's
                // collider and mesh are both disabled.
                crate_break_sound.Play();
                crate_collider.enabled = false;
                crate_mesh.enabled = false;
                fake_collider.SetActive(false);

                // If health_appear_guarantee is false...
                if (health_appear_guarantee == false)
                {
                    // ...then a random number from 1 to 4 is chosen...
                    int health_appear_freq = Random.Range(1, 4);
                    
                    // ...and if the number is 1, then a health pack item is spawned,
                    // placed directly on the floor.
                    if (health_appear_freq == 1)
                    {
                        Vector3 new_position = transform.position;
                        new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                        Instantiate(health_pack, new_position, Quaternion.identity);
                    }
                }
                else // if (health_appear_guarantee == true)
                {
                    // If health_appear_guarantee is true, then a health pack item is always spawned,
                    // placed directly on the floor.
                    Vector3 new_position = transform.position;
                    new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                    Instantiate(health_pack, new_position, Quaternion.identity);
                }

                // Finally, the crate object is destroyed.
                Destroy(gameObject, 1);
            }
        }
    }
}
