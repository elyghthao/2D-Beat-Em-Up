using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int rand ;
    public bool isHit;
    private bool _ready;

    private bool isAttacking;
    public ParticleSystem knockedOutParticle;
    public ParticleSystem healthGainParticle;


    
    void Start()
    {
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
        // Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
        Debug.Log(stateScript.CurrentState.ToString());
        // Debug.Log(stateScript.currentHealth);
        if(stateScript.CurrentState.ToString() == "PlayerAttackState") {
            isAttacking = true;
            isHit = false;


            if (stateScript.CurrentState.CurrentSubState.ToString() == "PlayerBlockState"){
                anim.Play("Block");
            }

            else if(lightAttack.activeSelf){
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
        }else if (isAttacking) {
            isAttacking = false;
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
                knockedOutParticle.Play();
                isHit = true;
            }
        }else if(stateScript.CurrentState.ToString() == "PlayerIdleState" && !isAttacking){
            // rand = 1;
            isHit = false;
            anim.Play("Idle");
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
