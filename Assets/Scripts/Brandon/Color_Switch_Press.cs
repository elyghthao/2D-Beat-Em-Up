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
    public MeshRenderer switch_mesh; // The color switch's mesh renderer.
    private bool switch_is_blue; // Whether the switch is currently blue or not.
    public Material switch_blue_sprite; // The sprite for the switch's blue state.
    public Material switch_orange_sprite; // The sprite for the switch's orange state.
    private Material shared_switch_sprite;

    // Start is called before the first frame update
    void Start()
    {
        // On start, the color switch's audio source and box collider
        // components are given to the associated variables for access.
        switch_press_sound = GetComponent<AudioSource>();
        switch_collider = GetComponent<BoxCollider>();

        switch_is_blue = true;

        shared_switch_sprite = switch_mesh.GetComponent<MeshRenderer>().sharedMaterial;
        shared_switch_sprite = switch_blue_sprite;
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
                // ...and the color switch is currently blue...
                if (switch_is_blue == true)
                {
                    // ...then switch_is_blue is set to false (because it turns orange)
                    // and the color switch's sprite is changed to the "orange" state sprite.
                    switch_is_blue = false;
                    // shared_switch_sprite = switch_orange_sprite;
                    Debug.Log("Turns Orange\n");
                }
                else // (switch_is_blue == false)
                {
                    // If the switch is orange, or not blue, then switch_is_blue is set to
                    // true (because it turns blue) and the color switch's sprite is set to
                    // the "blue" state sprite.
                    switch_is_blue = true;
                    // shared_switch_sprite = switch_blue_sprite;
                    Debug.Log("Turns Blue\n");
                }

                // Regardless of the state of the color switch, the switch press sound plays.
                switch_press_sound.Play();
            }
        }
    }
}
