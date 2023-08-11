using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door_Switch_Press : MonoBehaviour
{
    private AudioSource switch_pressed_sound; // The sound clip for when the door switch is pressed.
    private BoxCollider switch_collider; // The door switch's box collider.
    public MeshRenderer switch_mesh; // The door switch's mesh renderer.
    private bool switch_is_pressed; // Whether the door switch has already been pressed or not.
    private bool player_is_touching; // Whether the player is currently colliding with the door switch.
    public Material switch_on_sprite; // The sprite for the door switch's "on" state.
    public GameObject door; // The locked door that the door switch is linked to.
    private Color door_trans = new Color(1.0f, 1.0f, 1.0f, 0.4f);
    public Color pulse_color;
    public GameObject pulse_object;
    private Material pulse_material;
    private float pulse_time = 0;
    
    // Cached strings
    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int Timer = Shader.PropertyToID("_Timer");


    // Start is called before the first frame update
    void Start()
    {
        // On start, the door switch's audio source and box collider
        // components are given to the associated variables for access,
        // and switch_is_pressed and player_is_touching are set to false.
        switch_pressed_sound = GetComponent<AudioSource>();
        switch_collider = GetComponent<BoxCollider>();
        pulse_material = pulse_object.GetComponent<Renderer>().material;
        pulse_material.SetColor(Color, pulse_color);
        pulse_material.SetFloat(Timer, pulse_time);

        switch_is_pressed = false;
        player_is_touching = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is touching the door switch...
        if (player_is_touching)
        {
            if (!pulse_object.activeSelf) 
            {
                pulse_object.SetActive(true);
            }
            pulse_time += Time.deltaTime;
            // ...and they press the "E" key while the door switch has not
            // already been pressed...
            if (Input.GetKeyDown(KeyCode.E) && switch_is_pressed == false)
            {
                // ...then the door switch is activated.
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
        // If the door switch comes into contact with the player's collider,
        // then player_is_touching is set to true.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the door switch leaves contact with the player's collider,
        // then player_is_touching is set to false.
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            player_is_touching = false;
        }
    }

    void ActivateSwitch()
    {
        // The switch pressed sound effect plays, switch_is_pressed is set
        // to true (so that it can't be pressed again), and the door switch's
        // sprite is changed to the "on" state sprite.
        switch_pressed_sound.Play();
        switch_is_pressed = true;
        switch_mesh.material = switch_on_sprite;
        pulse_time = 0;

        // If a door is linked to the button, then the door is destroyed.
        if (door != null)
        {
            door.GetComponent<MeshRenderer>().material.color = door_trans;
            door.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
