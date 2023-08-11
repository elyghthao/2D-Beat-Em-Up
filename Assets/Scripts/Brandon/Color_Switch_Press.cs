using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Color_Switch_Press : MonoBehaviour
{
    private AudioSource switch_pressed_sound; // The sound clip for when the color switch is pressed.
    private BoxCollider switch_collider; // The color switch's box collider.
    public MeshRenderer switch_frame_mesh; // The color switch's frame's mesh renderer.
    public MeshRenderer switch_center_mesh; // The color switch's center mesh renderer.
    private Material shared_switch_center_material; // The center material shared between all color switches in the scene.
    private Color switch_blue_color = new Color(0.0f, 0.58f, 1.0f, 1.0f); // The color for the switch's blue state.
    private Color switch_orange_color = new Color(1.0f, 0.42f, 0.0f, 1.0f); // The color for the switch's orange state.
    private bool player_is_touching; // Whether the player is currently colliding with the color switch.
    private float trigger_timer; // The amount of pulse_time remaining until the player can no longer activate the switch.
    public float trigger_timer_max; // The maximum pulse_time at which the trigger timer starts at.
    public GameObject pulse_object;
    private Material pulse_material;
    private float pulse_time = 0;
    
    // Cached strings
    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int Timer = Shader.PropertyToID("_Timer");

    // Start is called before the first frame update
    void Start()
    {
        // On start, the color switch's audio source and box collider
        // components are given to the associated variables for access.
        switch_pressed_sound = GetComponent<AudioSource>();
        switch_collider = GetComponent<BoxCollider>();

        // The shared material of the center sprite of all the color switches in the scene starts out
        // in the blue state, and player_is_touching is set to false.
        shared_switch_center_material = switch_center_mesh.GetComponent<MeshRenderer>().sharedMaterial;
        shared_switch_center_material.color = switch_blue_color;
        player_is_touching = false;
        pulse_material = pulse_object.GetComponent<Renderer>().material;
        pulse_material.SetColor(Color, switch_blue_color);
        pulse_material.SetFloat(Timer, 0);

        // The trigger timer starts at a maximum of half a second.
        trigger_timer_max = 0.5f;
    }

    // Update is called once per frame
    void Update() {
        // If the trigger pulse_time reaches 0, then player_is_touching reverts to false
        // and the player can no longer activate the color switch.
        if (trigger_timer <= 0)
        {
            player_is_touching = false;
        }

        // If the player is touching the color switch...
        if (player_is_touching)
        {
            if (!pulse_object.activeSelf) 
            {
                pulse_object.SetActive(true);
            }
            pulse_time += Time.deltaTime;
            // ...then the trigger timer starts decreasing...
            trigger_timer -= Time.deltaTime;

            // ...and if the player presses the "E" key,
            // then the color switch is activated.
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateSwitch();
            }
        }
        else if (pulse_object.activeSelf)
        {
            pulse_object.SetActive(false);
            pulse_time = 0;
        }
        pulse_material.SetFloat(Timer, pulse_time);
    }

    void OnTriggerEnter(Collider other)
    {
        // If the color switch comes into contact with the player's collider,
        // then player_is_touching is set to true, and the trigger timer is
        // set back to its maximum.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = true;
            trigger_timer = trigger_timer_max;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // If the color switch comes into contact with the player's collider,
        // then player_is_touching is set to true, and the trigger timer is
        // set back to its maximum.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = true;
            trigger_timer = trigger_timer_max;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the color switch leaves contact with the player's collider,
        // then player_is_touching is set to false, and the trigger timer is
        // reduced to zero.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = false;
            trigger_timer = 0.0f;
        }
    }

    void ActivateSwitch()
    {
        // If the color switch's center is currently blue...
        if (shared_switch_center_material.color == switch_blue_color)
        {
            // ...then the center sprites of all of the color switches in the scene
            // are changed to the "orange" state color.
            shared_switch_center_material.color = switch_orange_color;
            pulse_material.SetColor(Color, switch_orange_color);
        }
        else if (shared_switch_center_material.color == switch_orange_color)
        {
            // If the switch is currently orange, then the center sprites of all
            // of the color switches in the scene are changed to the "blue" state color.
            shared_switch_center_material.color = switch_blue_color;
            pulse_material.SetColor(Color, switch_blue_color);
        }

        pulse_time = 0;
        // Regardless of the state of the color switch, the switch pressed sound effect plays.
        switch_pressed_sound.Play();
    }
}
