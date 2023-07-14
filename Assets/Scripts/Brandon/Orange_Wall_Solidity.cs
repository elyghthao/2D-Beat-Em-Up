using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange_Wall_Solidity : MonoBehaviour
{
    public GameObject color_switch;
    private Material color_switch_material;
    private BoxCollider orange_wall_collider; // The wall's box collider.
    public MeshRenderer orange_wall_mesh; // The wall's mesh renderer.

    // Start is called before the first frame update
    void Start()
    {
        orange_wall_collider = GetComponent<BoxCollider>();   
    }

    // Update is called once per frame
    void Update()
    { 
    }
}
