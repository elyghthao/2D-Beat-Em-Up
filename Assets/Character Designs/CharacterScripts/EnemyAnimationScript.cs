using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class EnemyAnimationScript : MonoBehaviour
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
    public int animNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        stateScript = this.gameObject.GetComponent<EnemyStateMachine>();
        StartCoroutine(checkStateReady());
        lightAttack = stateScript.lightAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
        isAttacking = false;
        currentPlayer = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        if (!_ready) return;
        try {
            // Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
            // Debug.Log(stateScript.CurrentState.ToString());
            // Debug.Log(stateScript.CurrentState.ToString() + ": " + stateScript.CurrentState.CurrentSubState.ToString());
            // Debug.Log(stateScript.currentHealth);
        }catch (Exception){
        }


        if(stateScript.CurrentState.ToString() == "EnemyAttackingState") {
            isAttacking = true;
            if (animNum == 0){
                if(lightAttack.activeSelf){
                    anim.Play("LightAttack");
                }else if(mediumAttack.activeSelf){
                    anim.Play("MediumAttack");
                }else if(slamAttack.activeSelf){
                    anim.Play("SlamAttack");
                }
            }else if(animNum == 1){
                if(lightAttack.activeSelf){
                    anim.Play("LightAttack1");
                }else if(mediumAttack.activeSelf){
                    anim.Play("MediumAttack1");
                }else if(slamAttack.activeSelf){
                    anim.Play("SlamAttack1");
                }
            }
            
        }else {
            isAttacking = false;
            animNum = UnityEngine.Random.Range(0,2);
        }





        if(stateScript.CurrentState.ToString() == "EnemyMovingState" && !isAttacking){//MOVING STATE
            if(stateScript.inPosition) {
                anim.Play("FightStance");
            }else {
                anim.Play("Walk");
            }
        }else if(stateScript.CurrentState.ToString() == "EnemyHurtState" ){//HURT STATE

                // Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
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
                        hitParticle.Play();
                    }
                    anim.Play("KnockedDown");

                }else if (stateScript.CurrentState.CurrentSubState.ToString() == "EnemySmackedState") {
                    anim.Play("Hurt", -1, 0f);
                    
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
}
