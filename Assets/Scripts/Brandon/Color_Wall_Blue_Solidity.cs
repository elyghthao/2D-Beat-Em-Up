using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Color_Wall_Blue_Solidity : MonoBehaviour
{
    public GameObject switch_object; // The color switch prefab.
    private GameObject switch_center_object; // The object containing the mesh for the color switch's center.
    private Material switch_center_material; // The material used for the center of all the color switches.
    private BoxCollider blue_wall_collider; // The blue wall's box collider.
    private Renderer blue_wall_mesh; // The blue wall's renderer.
    private Material shared_blue_wall_material; // The material shared between all of the blue walls in the scene.
    private Color blue_visible = new Color(0.0f, 0.58f, 1.0f, 1.0f); // The color for the blue wall's visible state.
    private Color blue_trans = new Color(0.0f, 0.58f, 1.0f, 0.4f); // The color for the blue wall's transparent state.
    private NavMeshObstacle navMeshObs; // For the nav mesh

    // Start is called before the first frame update
    void Start()
    {
        // On start, the blue wall's box collider and renderer components
        // are given to the associated variables for access.
        blue_wall_collider = GetComponent<BoxCollider>();
        blue_wall_mesh = GetComponent<Renderer>();

        // The color switch's second child object, containing the material for the switch's center,
        // is stored into switch_center_object, and the the material shared by the centers of all the color
        // switches is stored into switch_center_material.
        switch_center_object = switch_object.transform.GetChild(1).gameObject;
        switch_center_material = switch_center_object.GetComponent<MeshRenderer>().sharedMaterial;

        // The shared material used by all of the blue walls in the scene is stored and
        // starts out blue and visible, and the blue wall's collider starts enabled.
        shared_blue_wall_material = blue_wall_mesh.sharedMaterial;
        shared_blue_wall_material.color = blue_visible;
        blue_wall_collider.enabled = true;

        // For nav mesh
        navMeshObs = gameObject.GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the shared color of all the color switches is blue, then the blue walls all
        // turn visible and their colliders are enabled. Otherwise, the blue walls all turn
        // transparent and their colliders are disabled.
        if (switch_center_material.color == blue_visible)
        {
            shared_blue_wall_material.color = blue_visible;
            blue_wall_collider.enabled = true;
            navMeshObs.enabled = true;
        }
        else
        {
            shared_blue_wall_material.color = blue_trans;
            blue_wall_collider.enabled = false;
            navMeshObs.enabled = false;
        } 
    }
}
