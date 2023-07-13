using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_Switch_Press : MonoBehaviour
{
    // The list of attack hitbox tags to check for collision with the color switch.
    private string[] tags_to_check = {"FirstLightAttack", "SecondLightAttack", "ThirdLightAttack",
                                    "FirstMediumAttack", "SecondMediumAttack", "SlamAttack", "Attack Hitbox"};
    private AudioSource switch_press_sound; // The sound clip for when the color switch is pressed.
    private BoxCollider switch_collider; // The color switch's box collider.
    public MeshRenderer switch_frame_mesh; // The color switch's frame's mesh renderer.
    public MeshRenderer switch_center_mesh; // The color switch's center mesh renderer.
    private Material shared_switch_center_material; // The center material shared between all color switches in the scene.
    private Color switch_blue_color = new Color(0.0f, 0.58f, 1.0f, 1.0f); // The color for the switch's blue state.
    private Color switch_orange_color = new Color(1.0f, 0.42f, 0.0f, 1.0f); // The color for the switch's orange state.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the color switch's audio source and box collider
        // components are given to the associated variables for access.
        switch_press_sound = GetComponent<AudioSource>();
        switch_collider = GetComponent<BoxCollider>();

        // The shared material of the center sprite of all the color switches in the scene starts out
        // in the blue state.
        shared_switch_center_material = switch_center_mesh.GetComponent<MeshRenderer>().sharedMaterial;
        shared_switch_center_material.color = switch_blue_color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Every attack hitbox tag is used to check collision between the color switch
        // and the player's attack hitboxes.
        foreach (string tag in tags_to_check)
        {
            // If the color switch comes into contact with the player's attack hitbox,
            // signified by the currently compared tag...
            if (other.gameObject.CompareTag(tag))
            {
                // ...and the color switch's center is currently blue...
                if (shared_switch_center_material.color == switch_blue_color)
                {
                    // ...then the center sprites of all of the color switches in the scene
                    // are changed to the "orange" state color.
                    shared_switch_center_material.color = switch_orange_color;
                }
                else if (shared_switch_center_material.color == switch_orange_color)
                {
                    // If the switch is currently orange, then the center sprites of all
                    // of the color switches in the scene are changed to the "blue" state color.
                    shared_switch_center_material.color = switch_blue_color;
                }

                // Regardless of the state of the color switch, the switch press sound plays.
                switch_press_sound.Play();
            }
        }
    }
}
