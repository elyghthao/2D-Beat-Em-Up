using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steel_Crate : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the steel crate.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource crate_sfx; // The audio source component used for the steel crate.
    private BoxCollider crate_collider; // The steel crate's box collider.
    private MeshRenderer crate_mesh; // The steel crate's mesh renderer.
    private int crate_health; // The current remaining health of the steel crate.
    public bool health_appear_guarantee; // Whether it is guaranteed that a health pack will spawn from the steel crate.
    public GameObject health_pack; // The health pack prefab.
    public AudioClip crate_break_sound; // The sound clip for when the steel crate is destroyed.
    public Material crate_cracked_1; // The material for the steel crate's first cracked state.
    public Material crate_cracked_2; // The material for the steel crate's second cracked state.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the steel crate's audio source, box collider, and mesh renderer
        // components are given to the associated variables for access.
        crate_sfx = GetComponent<AudioSource>();
        crate_collider = GetComponent<BoxCollider>();
        crate_mesh = GetComponent<MeshRenderer>();

        // The steel crate always starts with a health value of 3, meaning
        // player must attack it three times to destroy it.
        crate_health = 3;
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void OnTriggerEnter(Collider other)
    {
        // Every attack hitbox tag is used to check collision between the
        // steel crate and the player's attack hitboxes.
        foreach (string tag in tags_to_check)
        {
            // If the steel crate comes into contact with the player's attack
            // hitbox, signified by the currently compared tag...
            if (other.gameObject.CompareTag(tag))
            {
                // ...and the steel crate currently has only 1 health point remaining...
                if (crate_health == 1)
                {
                    // ...then the crate's audio clip changes to the crate breaking sound...
                    crate_sfx.clip = crate_break_sound;

                    // ...and then the crate breaking sound plays, and the steel crate's
                    // collider and mesh renderer are both disabled.
                    crate_sfx.Play();
                    crate_collider.enabled = false;
                    crate_mesh.enabled = false;

                    // If health_appear_guarantee is set to false...
                    if (health_appear_guarantee == false)
                    {
                        // ...then a random number from 1 to 4 is chosen.
                        int health_appear_freq = Random.Range(1, 4);
                    
                        // If the chosen number is 1, then a health pack object is instantiated
                        // on the ground directly where the steel crate was. Thus, there is a
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
                        // is always instantiated on the ground directly where the steel crate was.
                        Vector3 new_position = transform.position;
                        new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                        Instantiate(health_pack, new_position, Quaternion.identity);
                    }

                    // Finally, the steel crate object is destroyed.
                    Destroy(gameObject, 1);
                }
                else // if (crate_health > 1)
                {
                    // If the steel crate's health is still greater than 1, then its
                    // remaining health decrements and the crate hurt sound plays.
                    crate_health--;
                    crate_sfx.Play();

                    // If the steel crate has 2 health remaining, then its material changes
                    // to the first cracked steel crate material. If the steel crate has
                    // 1 health remaining, then its material changes to the second cracked
                    // steel crate material.
                    if (crate_health == 2)
                    {
                        crate_mesh.material = crate_cracked_1;
                    }
                    
                    if (crate_health == 1)
                    {
                        crate_mesh.material = crate_cracked_2;
                    }
                }
            }
        }
    }
}
