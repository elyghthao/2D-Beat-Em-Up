using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wooden_Crate : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the crate.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource crate_break_sound; // The sound clip for when the crate is destroyed.
    private BoxCollider crate_collider; // The crate's box collider.
    private MeshRenderer crate_mesh; // The crate's mesh renderer.
    public bool health_appear_guarantee; // Whether it is guaranteed that a health pack will spawn from the crate.
    public GameObject health_pack; // The health pack prefab.

    // Start is called before the first frame update
    void Start()
    {
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
        foreach (string tag in tags_to_check)
        {
            if (other.gameObject.CompareTag(tag))
            {
                crate_break_sound.Play();
                crate_collider.enabled = false;
                crate_mesh.enabled = false;

                if (health_appear_guarantee == false)
                {
                    int health_appear_freq = Random.Range(1, 4);

                    if (health_appear_freq == 1)
                    {
                        Vector3 new_position = transform.position;
                        new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                        Instantiate(health_pack, new_position, Quaternion.identity);
                    }
                }
                else // if (health_appear_guarantee == true)
                {
                    Vector3 new_position = transform.position;
                    new_position = new Vector3(new_position.x, 0.5f, new_position.z);
                    Instantiate(health_pack, new_position, Quaternion.identity);
                }

                Destroy(gameObject, 1);
            }
        }
    }
}
