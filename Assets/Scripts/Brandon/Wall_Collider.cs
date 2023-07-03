using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Collider : MonoBehaviour
{
    private BoxCollider wall_collider; // The wall's box collider.
    public MeshRenderer wall_mesh; // The wall's mesh renderer.

    // Start is called before the first frame update
    void Start()
    {
        wall_collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {   
    }
}
