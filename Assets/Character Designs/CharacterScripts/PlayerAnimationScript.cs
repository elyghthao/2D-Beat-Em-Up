using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update



    
    void Start()
    {
        anim = this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("LightAttack"));
        if (Input.GetKeyDown(KeyCode.A)) {
            anim.Play("LightAttack");
        }else if (anim.GetCurrentAnimatorStateInfo(0).IsName("LightAttack") && Input.GetKeyDown(KeyCode.A)){
            anim.Play("LightAttack2");



        }else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("LightAttack")){
            if (Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.UpArrow)||
            Input.GetKey(KeyCode.LeftArrow)||
            Input.GetKey(KeyCode.RightArrow) ){
                anim.Play("Walk");
            }else {
                anim.Play("Idle");
            }
        }


        
        





        
    }
}
