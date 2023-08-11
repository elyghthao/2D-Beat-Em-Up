using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationScript : MonoBehaviour
{
    public PlayerStateMachine stateScript;
    public Animator anim;
    public GameObject lightAttack;
    public GameObject lightAttack1;
    public GameObject lightAttack2;
    public GameObject mediumAttack;
    public GameObject mediumAttack1;
    // public GameObject mediumAttack2;// add this later, talk with vincent
    public GameObject slamAttack;
    // private int rand;
    public bool isHit;
    private bool _ready;

    private bool isAttacking;
    public ParticleSystem knockedOutParticle;
    public ParticleSystem healthGainParticle;
    public AudioSource hurtSound;


    
    void Start()
    {
        hurtSound = this.gameObject.GetComponent<AudioSource>();
        stateScript = this.gameObject.GetComponent<PlayerStateMachine>();
        StartCoroutine(checkStateReady());
        lightAttack = stateScript.lightAttackBounds;
        lightAttack1 = stateScript.lightFirstFollowupAttackBounds;
        lightAttack2 = stateScript.lightSecondFollowupAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        mediumAttack1 = stateScript.mediumFirstFollowupAttackBounds;
        // mediumAttack2 = stateScript.mediumSecondFollowupAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
        isAttacking = false;
        isHit = false;
    }

    // Update is called once per frame
    void Update() {
        if (!_ready) return;
        try {
            // Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
            // Debug.Log(stateScript.CurrentState.ToString());
            // Debug.Log(stateScript.CurrentState.ToString() + ": " + stateScript.CurrentState.CurrentSubState.ToString());
            // Debug.Log(stateScript.currentHealth);
            // Debug.Log(stateScript.KnockdownMeter);
        }catch (Exception){
        }


        if(stateScript.CurrentState.ToString() == "PlayerAttackState") {
            isAttacking = true;
            isHit = false;

            try {
                if (stateScript.CurrentState.CurrentSubState.ToString() == "PlayerBlockState"){
                    anim.Play("Block");
                }else if(lightAttack.activeSelf){
                    anim.Play("LightAttack");
                }else if(lightAttack1.activeSelf){
                    anim.Play("LightAttack1");
                }else if(lightAttack2.activeSelf){
                    anim.Play("LightAttack2");
                }else if(mediumAttack.activeSelf){
                    anim.Play("MediumAttack");
                }else if(mediumAttack1.activeSelf){
                    anim.Play("MediumAttack1");
                }
                // else if(mediumAttack2.activeSelf){
                //     anim.Play("MediumAttack2");
                // }
                else if(slamAttack.activeSelf){
                    anim.Play("SlamAttack");
                }
                } catch(Exception ){
                    
                }
        }else if (isAttacking) {
            isAttacking = false;
        }


        
        
        
        if(stateScript.CurrentState.ToString() == "PlayerMoveState" && !isAttacking){
            // rand = 1;
            isHit = false;
            anim.Play("Walk");
        }else if(stateScript.CurrentState.ToString() == "PlayerHurtState" ){
            if (stateScript.CurrentState.CurrentSubState.ToString() == "PlayerRecoveryState") {
                anim.Play("Recover");
            }else if (stateScript.KnockedDown){
                
                if(stateScript.CurrentState.CurrentSubState.ToString() == "PlayerKnockedDownState") {//this makes the particle effect
                    hurtSound.PlayOneShot(hurtSound.clip);
                    knockedOutParticle.Play();
                }
                anim.Play("KnockedDown");

            }else if (stateScript.CurrentState.CurrentSubState.ToString() == "PlayerSmackedState") {
                // anim.Play("Idle");
                hurtSound.PlayOneShot(hurtSound.clip);
                anim.Play("Hurt", -1, 0f);
            }else {//add more code to account repeatedly getting hit
                //Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
                // anim.Play("Idle");
            }
        }else if(stateScript.CurrentState.ToString() == "PlayerIdleState" && !isAttacking){
            // rand = 1;
            isHit = false;
            anim.Play("Idle");
        }else if (stateScript.CurrentState.ToString() == "PlayerDeathState"){
            anim.Play("Death");
        }
        


        if(stateScript.gotHealed){
            stateScript.gotHealed = false;
            healthGainParticle.Play();
        }
        


        
        





        
    }
    IEnumerator checkStateReady() {
        while (!stateScript.FinishedInitialization) {
            yield return null;
        }
        _ready = true;
    }
    
}
