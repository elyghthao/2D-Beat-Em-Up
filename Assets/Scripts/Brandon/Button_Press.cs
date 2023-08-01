using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button_Press : MonoBehaviour
{
    private AudioSource button_press_sound; // The sound clip for when the button is pressed.
    private BoxCollider button_collider; // The button's box collider.
    public MeshRenderer button_mesh; // The button's mesh renderer.
    private bool button_is_pressed; // Whether the button has already been pressed or not.
    private bool player_is_touching; // Whether the player is currently colliding with the button.
    public Material button_on_sprite; // The sprite for the button's "on" state.
    public GameObject door; // The locked door that the button is linked to.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the button's audio source and box collider
        // components are given to the associated variables for access,
        // and button_is_pressed and player_is_touching are set to false.
        button_press_sound = GetComponent<AudioSource>();
        button_collider = GetComponent<BoxCollider>();
        button_is_pressed = false;
        player_is_touching = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is touching the button...
        if (player_is_touching == true)
        {
            // ...and they press the "E" key while the button has not
            // already been pressed...
            if (Input.GetKeyDown(KeyCode.E) && button_is_pressed == false)
            {
                // ...then the switch is activated.
                ActivateSwitch();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the button comes into contact with the player's collider,
        // then player_is_touching is set to true.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the button leaves contact with the player's collider,
        // then player_is_touching is set to false.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = false;
        }
    }

    void ActivateSwitch()
    {
        // The button pressing sound effect plays, button_is_pressed is set
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
