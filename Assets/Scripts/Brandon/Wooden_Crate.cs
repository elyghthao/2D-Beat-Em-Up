using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wooden_Crate : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the wooden crate.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource crate_break_sound; // The sound clip for when the wooden crate is destroyed.
    private BoxCollider crate_collider; // The wooden crate's box collider.
    private MeshRenderer crate_mesh; // The woode crate's mesh renderer.
    public bool health_appear_guarantee; // Whether it is guaranteed that a health pack will spawn from the wooden crate.
    public GameObject health_pack; // The health pack prefab.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the wooden crate's audio source, box collider, and mesh renderer
        // components are given to the associated variables for access.
        crate_break_sound = GetComponent<AudioSource>();
        crate_collider = GetComponent<BoxCollider>();
        crate_mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Every attack hitbox tag is used to check collision between the
        // wooden crate and the player's attack hitboxes.
        foreach (string tag in tags_to_check)
        {
            // If the wooden crate comes into contact with the player's attack
            // hitbox, signified by the currently compared tag...
            if (other.gameObject.CompareTag(tag))
            {
                // ...then the wooden crate breaking sound effect plays, and the wooden
                // crate's collider and mesh renderer are both disabled.
                crate_break_sound.Play();
                crate_collider.enabled = false;
                crate_mesh.enabled = false;

                // If health_appear_guarantee is set to false...
                if (health_appear_guarantee == false)
                {
                    // ...then a random number between 1 and 4 is chosen.
                    int health_appear_freq = Random.Range(1, 4);

                    // If the chosen number is 1, then a health pack object is instantiated
                    // on the ground directly where the wooden crate was. Thus, there is a
                    // 25% chance of a health pack spawning.
                    if (health_appear_freq == 1)
                    {
                        Vector3 new_position = transform.position;
                        new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                        Instantiate(health_pack, new_position, Quaternion.identity);
                    }
                }
                else // if (health_appear_guarantee == true)
                {
                    // If health_appear_guarantee is set to true, then a health pack object
                    // is always instantiated on the ground directly where the wooden crate was.
                    Vector3 new_position = transform.position;
                    new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                    Instantiate(health_pack, new_position, Quaternion.identity);
                }

                // Finally, the wooden crate object is destroyed.
                Destroy(gameObject, 1);
            }
        }
    }
}
