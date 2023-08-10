using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class FinalBossAnimationScript : MonoBehaviour
{
    public EnemyStateMachine stateScript;
    public Animator anim;
    public GameObject lightAttack;
    public GameObject mediumAttack;
    public GameObject slamAttack;
    public bool isAttacking;
    public ParticleSystem hitParticle;
    private bool _ready;
    private GameObject currentPlayer;
    public GameObject body;
    public int lightAttackNum = 0;
    public int mediumAttackNum = 0;
    public int heavyAttackNum = 0;
    public bool lastLight = false;
    public bool lastMed = false;
    public SpriteEffects spriteEffects;
    public AudioSource hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        hurtSound = this.gameObject.GetComponent<AudioSource>();
        stateScript = this.gameObject.GetComponent<EnemyStateMachine>();
        StartCoroutine(checkStateReady());
        lightAttack = stateScript.lightAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
        isAttacking = false;
        currentPlayer = GameObject.FindWithTag("Player");
        spriteEffects = this.gameObject.GetComponent<SpriteEffects>();
    }

    // Update is called once per frame
    void Update() {
        if (!_ready) return;
        try {
            // Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
            Debug.Log(stateScript.CurrentState.ToString());
            // Debug.Log(stateScript.CurrentState.ToString() + ": " + stateScript.CurrentState.CurrentSubState.ToString());
            // Debug.Log(stateScript.currentHealth);
        }catch (Exception){
        }


        if(stateScript.CurrentState.ToString() == "EnemyAttackingState") {
            isAttacking = true;
            if(stateScript.isBlocking){
                lastLight = false;
                lastMed = false;
                anim.Play("Block");
            }else if(lightAttack.activeSelf){
                lastLight = true;
                lastMed = false;

                if(lightAttackNum == 0){
                    anim.Play("LightAttack");
                }else if(lightAttackNum == 1){
                    anim.Play("LightAttack1");
                }else if(lightAttackNum == 2){
                    anim.Play("LightAttack2");
                }

                // lightAttackNum = (lightAttackNum + 1) % 3 ;
                
            }else if(mediumAttack.activeSelf){
                lastLight = false;
                lastMed = true;
                if(mediumAttackNum == 0){
                    anim.Play("MediumAttack");
                }else if (mediumAttackNum == 1){
                    anim.Play("MediumAttack1");
                }
                
            }else if(slamAttack.activeSelf){
                spriteEffects.doEffect("Slam");
                spriteEffects.doEffect("Direction");
                spriteEffects.doEffect("Direction2");
                anim.Play("SlamAttack");
            }





            
            
        }else {
            isAttacking = false;
            if (lastLight){
                lightAttackNum = (lightAttackNum + 1) % 3 ;
                lastLight = false;
            }
            if(lastMed){
                mediumAttackNum = (mediumAttackNum + 1) % 2 ;
                lastMed = false;
            }
        }





        if(stateScript.CurrentState.ToString() == "EnemyMovingState" && !isAttacking){//MOVING STATE
            
            if(stateScript.inPosition) {
                anim.Play("FightStance");
            }else {
                anim.Play("Walk");
            }
            
        }else if(stateScript.CurrentState.ToString() == "EnemyHurtState" ){//HURT STATE
                resetAnimNum();
                Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
                if (stateScript.CurrentState.CurrentSubState.ToString() == "EnemyDeathState"){
                    if(stateScript.KnockedDown){
                        anim.Play("DeathFromDown");
                    }else {
                        anim.Play("Death");
                    }
                }else if (stateScript.CurrentState.CurrentSubState.ToString() == "EnemyRecoveryState") {
                    anim.Play("Recover");
                }else if (stateScript.KnockedDown){
                    if(stateScript.CurrentState.CurrentSubState.ToString() == "EnemyKnockedDownState") {//this makes the particle effect
                        // hurtSound.PlayOneShot(hurtSound.clip);
                        hitParticle.Play();
                    }
                    anim.Play("KnockedDown");

                }else if (stateScript.CurrentState.CurrentSubState.ToString() == "EnemySmackedState") {
                    anim.Play("Hurt", -1, 0f);
                    // hurtSound.PlayOneShot(hurtSound.clip);
                }
        }else if(stateScript.CurrentState.ToString() == "EnemyIdleState" && !isAttacking){//IDLE STATE
            anim.Play("Idle");
        }



    }

    IEnumerator checkStateReady() {
        while (!stateScript.FinishedInitialization) {
            Debug.Log("StuckInAnimationCheck");
            yield return null;
        }
        _ready = true;
    }

    private void resetAnimNum(){
        lightAttackNum = 0;
        mediumAttackNum = 0;
    }
}
