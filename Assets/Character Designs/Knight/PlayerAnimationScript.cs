using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    private Animator anim;
    public GameObject flipBone;
    public bool isFacingRight = true;
    // Start is called before the first frame update



    private float scaleX;
    private float scaleY;
    private float scaleZ;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("LightAttack"));
        if (Input.GetKeyDown(KeyCode.A)) {
            anim.Play("LightAttack");
        }else if (anim.GetCurrentAnimatorStateInfo(0).IsName("LightAttack") && Input.GetKeyDown(KeyCode.A)){
            anim.Play("LightAttack2");



        }else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("LightAttack")){
            if (Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.UpArrow)||
            Input.GetKey(KeyCode.LeftArrow)||
            Input.GetKey(KeyCode.RightArrow) ){
                anim.Play("KnightWalking");
            }else {
                anim.Play("KnightIdle");
            }
        }


        
        if (Input.GetKey(KeyCode.LeftArrow)){//orientation
            isFacingRight = false;
        }else if (Input.GetKey(KeyCode.RightArrow)){
            isFacingRight = true;
        }





        scaleX = transform.localScale[0];
        scaleY = transform.localScale[1];
        scaleZ = transform.localScale[2];
        if(!isFacingRight) {//is facing left
            flipBone.transform.localScale = new Vector3(1, -1, 1);
        }else {
            flipBone.transform.localScale = new Vector3(1,1,1);
        }
    }
}
