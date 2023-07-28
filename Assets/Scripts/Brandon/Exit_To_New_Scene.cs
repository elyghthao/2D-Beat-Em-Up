using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit_To_New_Scene : MonoBehaviour
{
    public string destination; // The name of the destination scene.
    private Scene current_scene; // The currently loaded scene.
    private BoxCollider exit_collider; // The exit's box collider.
    public MeshRenderer exit_mesh; // The exit's mesh renderer.

    // Start is called before the first frame update
    void Start()
    {
        // On start, the exit's box collider component is given to the
        // associated variable for access, and the currently loaded scene
        // is given to current_scene.
        exit_collider = GetComponent<BoxCollider>();
        current_scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // If the exit comes into contact with the player's collision,
        // signified by the tag "Player"...
        if (other.gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            // ...and if the destination string is empty...
            if (destination == "")
            {
                // ...then the currently loaded scene is loaded again.
                SceneManager.LoadScene(current_scene.name);
            }
            else
            {
                // Otherwise, the scene with the same name as the destination
                // string is loaded.
                SceneManager.LoadScene(destination);   
            }
        }
    }
}
