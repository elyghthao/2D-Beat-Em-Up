using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steel_Crate_Destroy : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the steel crate.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource crate_sfx; // The audio source used for the steel crate.
    private BoxCollider crate_collider; // The steel crate's box collider.
    public MeshRenderer crate_mesh; // The steel crate's mesh renderer.
    public bool health_appear_guarantee; // Whether it is guaranteed that a health pack will spawn from the steel crate.
    public int crate_health; // How many hits it takes to break the crate, minimum of 2.
    public GameObject health_pack; // The health pack prefab.
    public AudioClip crate_break_sound; // The sound clip for when the steel crate is broken.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the steel crate's audio source and box collider
        // components are given to the associated variables for access.
        crate_sfx = GetComponent<AudioSource>();
        crate_collider = GetComponent<BoxCollider>();

        // If the crate health has a value less than 2, then it is changed
        // to a default value of 2.
        if (crate_health < 2)
        {
            crate_health = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Every attack hitbox tag is used to check collision between the steel crate
        // and the player's attack hitboxes.
        foreach (string tag in tags_to_check)
        {
            // If the steel crate comes into contact with the player's attack hitbox,
            // signified by the currently compared tag...
            if (other.gameObject.CompareTag("Attack Hitbox"))
            {
                // ...and the steel crate currently has only 1 health point remaining...
                if (crate_health == 1)
                {
                    // ...then the crate's audio clip changes to the crate breaking sound...
                    crate_sfx.clip = crate_break_sound;

                    // ...and then the crate breaking sound plays, and the steel crate's
                    // collider and mesh are both disabled.
                    crate_sfx.Play();
                    crate_collider.enabled = false;
                    crate_mesh.enabled = false;

                    // If health_appear_guarantee is false...
                    if (health_appear_guarantee == false)
                    {
                        // ...then a random number from 1 to 4 is chosen...
                        int health_appear_freq = Random.Range(1, 4);
                    
                        // ...and if the number is 1, then a health pack item is spawned.
                        if (health_appear_freq == 1)
                        {
                            Instantiate(health_pack, transform.position, Quaternion.identity);
                        }
                    }
                    else // if (health_appear_guarantee == true)
                    {
                        // If health_appear_guarantee is true, then a health pack item is always spawned.
                        Instantiate(health_pack, transform.position, Quaternion.identity);
                    }

                    // Finally, the steel crate object is destroyed.
                    Destroy(gameObject, 1);
                }
                else // if (crate_health > 1)
                {
                    // If the crate's health is still greater than 1, then its health
                    // decrements and the crate hurt sound plays.
                    crate_health--;
                    crate_sfx.Play();
                }
            }
        }
    }
}
