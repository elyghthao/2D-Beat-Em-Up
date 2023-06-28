using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    public bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) ||
        Input.GetKey(KeyCode.UpArrow) ||
        Input.GetKey(KeyCode.LeftArrow) ||
        Input.GetKey(KeyCode.RightArrow)){
            anim.Play("WalkLoop");
            Debug.Log("walking");
        }else {
            anim.Play("Idle");
        }

        if(Input.GetKey(KeyCode.LeftArrow) &&
        facingRight) {
            //swap
            this.transform.localScale = new Vector3(-4,3,6);
            facingRight = false;
        }

        if(Input.GetKey(KeyCode.RightArrow) &&
        !facingRight) {
            this.transform.localScale = new Vector3(4,3,6);
            facingRight = true;
        }
    }
}
