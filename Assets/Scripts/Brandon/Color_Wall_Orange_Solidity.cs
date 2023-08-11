using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Color_Wall_Orange_Solidity : MonoBehaviour
{
    public GameObject switch_object; // The color switch prefab.
    private GameObject switch_center_object; // The object containing the mesh for the color switch's center.
    private Material switch_center_material; // The material used for the center of all the color switches.
    private BoxCollider orange_wall_collider; // The orange wall's box collider.
    private Renderer orange_wall_mesh; // The orange wall's renderer.
    private Material shared_orange_wall_material; // The material shared between all of the orange walls in the scene.
    private Color orange_visible = new Color(1.0f, 0.42f, 0.0f, 1.0f); // The color for the orange wall's visible state.
    private Color orange_trans = new Color(1.0f, 0.42f, 0.0f, 0.4f); // The color for the orange wall's transparent state.
    private NavMeshObstacle navMeshObs; // For the nav mesh

    // Start is called before the first frame update
    void Start()
    {
        // On start, the orange wall's box collider and renderer components
        // are given to the associated variables for access.
        orange_wall_collider = GetComponent<BoxCollider>();
        orange_wall_mesh = GetComponent<Renderer>();

        // The color switch's second child object, containing the material for the switch's center,
        // is stored into switch_center_object, and the the material shared by the centers of all the color
        // switches is stored into switch_center_material.
        switch_center_object = switch_object.transform.GetChild(1).gameObject;
        switch_center_material = switch_center_object.GetComponent<MeshRenderer>().sharedMaterial;

        // The shared material used by all of the orange walls in the scene is stored and
        // starts out orange and transparent, and the orange wall's collider starts disabled.
        shared_orange_wall_material = orange_wall_mesh.sharedMaterial;
        shared_orange_wall_material.color = orange_trans;
        orange_wall_collider.enabled = false;

        // For nav mesh
        navMeshObs = gameObject.GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the shared color of all the color switches is orange, then the orange walls all
        // turn visible and their colliders are enabled. Otherwise, the orange walls all turn
        // transparent and their colliders are disabled.
        if (switch_center_material.color == orange_visible)
        {
            shared_orange_wall_material.color = orange_visible;
            orange_wall_collider.enabled = true;
            navMeshObs.enabled = true;
        }
        else
        {
            shared_orange_wall_material.color = orange_trans;
            orange_wall_collider.enabled = false;
            navMeshObs.enabled = false;
        } 
    }
}
