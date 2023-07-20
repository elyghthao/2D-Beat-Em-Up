using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    public PlayerStateMachine stateScript;
    public Animator anim;
    public GameObject lightAttack;
    public GameObject mediumAttack;
    public GameObject slamAttack;
    int rand ;
    public bool isHit;


    private bool isAttacking;
    public ParticleSystem hitParticle;


    
    void Start()
    {
        stateScript = this.gameObject.GetComponent<PlayerStateMachine>();
        lightAttack = stateScript.lightAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
        isAttacking = false;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("rand num: " + rand);
        // Debug.Log(stateScript.CurrentState.ToString());

        if(stateScript.CurrentState.ToString() == "PlayerAttackState") {
            isAttacking = true;
            isHit = false;

            if(lightAttack.activeSelf){
                if (rand == 1){
                    anim.Play("LightAttack");
                }else {
                    anim.Play("LightAttack1");
                }
        }else if(mediumAttack.activeSelf){
            
            if (rand == 1){
                    anim.Play("MediumAttack");
                }else {
                    anim.Play("MediumAttack1");
                }

        }else if(slamAttack.activeSelf){
            anim.Play("SlamAttack");
        }
        }else if (isAttacking) {
            isAttacking = false;
            rand = (rand + 1) % 2;
        }


        
        
        
        if(stateScript.CurrentState.ToString() == "PlayerMoveState" && !isAttacking){
            rand = 1;
            isHit = false;
            anim.Play("Walk");
        }else if(stateScript.CurrentState.ToString() == "PlayerHurtState" ){
            anim.Play("Hurt");
            if(!isHit){
                rand = 1;
                anim.Play("Hurt");
                hitParticle.Play();
                isHit = true;
            }
        }else if(stateScript.CurrentState.ToString() == "PlayerIdleState" && !isAttacking){
            // rand = 1;
            isHit = false;
            anim.Play("Idle");
        }
        
        


        
        





        
    }

    
}
