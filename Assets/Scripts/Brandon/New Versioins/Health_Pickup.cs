using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pickup : MonoBehaviour
{
    private AudioSource health_pack_collect_sound; // The sound clip for when the health pack is picked up.
    private BoxCollider health_pack_collider; // The health pack's box collider.
    private MeshRenderer health_pack_mesh; // The health pack's mesh renderer.

    // Start is called before the first frame update
    void Start()
    {
        health_pack_collect_sound = GetComponent<AudioSource>();
        health_pack_collider = GetComponent<BoxCollider>();
        health_pack_mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            health_pack_collect_sound.Play();
            health_pack_collider.enabled = false;
            health_pack_mesh.enabled = false;

            GameObject player = GameObject.Find("Player");
            PlayerStateMachine player_machine = player.GetComponent<PlayerStateMachine>();
            player_machine.HealCharacter(35);

            Destroy(gameObject, 1);
        }
    }
}
