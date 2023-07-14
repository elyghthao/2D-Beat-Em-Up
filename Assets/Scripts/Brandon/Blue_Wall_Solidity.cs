using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Wall_Solidity : MonoBehaviour
{
    public GameObject color_switch;
    private Material color_switch_material;
    private BoxCollider blue_wall_collider; // The blue wall's box collider.
    public MeshRenderer blue_wall_mesh; // The blue wall's mesh renderer.

    // Start is called before the first frame update
    void Start()
    {
        blue_wall_collider = GetComponent<BoxCollider>();   
    }

    // Update is called once per frame
    void Update()
    {
    }
}
