using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Press : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the button.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource button_press_sound; // The sound clip for when the button is pressed.
    private BoxCollider button_collider; // The button's box collider.
    public MeshRenderer button_mesh; // The button's mesh renderer.
    private bool button_is_pressed; // Whether the button has already been pressed or not.
    public Material button_on_sprite; // The sprite for the button's "on" state.
    public GameObject door; // The locked door that the button is linked to.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the button's audio source and box collider
        // components are given to the associated variables for access,
        // and button_is_pressed is set to false.
        button_press_sound = GetComponent<AudioSource>();
        button_collider = GetComponent<BoxCollider>();
        button_is_pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Every attack hitbox tag is used to check collision between the button
        // and the player's attack hitboxes.
        foreach (string tag in tags_to_check)
        {
            // If the crate comes into contact with the player's attack hitbox,
            // signified by the currently compared tag, and the button is not already pressed...
            if (other.gameObject.CompareTag(tag) && button_is_pressed == false)
            {
                // ...then the button pressing sound effect plays, button_is_pressed is set
                // to true (so that it can't be pressed again), and the button's sprite is
                // changed to the "on" state sprite.
                button_press_sound.Play();
                button_is_pressed = true;
                button_mesh.material = button_on_sprite;

                // If a door is linked to the button, then the door is destroyed.
                if (door != null)
                {
                    Destroy(door);
                }
            }
        }
    }
}
