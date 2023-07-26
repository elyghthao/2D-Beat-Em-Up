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
    private bool gotKnockedOut;
    
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
        gotKnockedOut = false;
    }

    // Update is called once per frame
    void Update() {
        if (!_ready) return;
        try {
            // Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
            // Debug.Log(stateScript.CurrentState.ToString());
            // Debug.Log(stateScript.currentHealth);
        }catch (Exception e){
            Debug.Log(e);
        }
        

        if(stateScript.CurrentState.ToString() == "EnemyAttackingState") {
            isAttacking = true;
            if(lightAttack.activeSelf){
                anim.Play("LightAttack");
            }else if(mediumAttack.activeSelf){
                anim.Play("MediumAttack");
            }else if(slamAttack.activeSelf){
                anim.Play("SlamAttack");
            }
            }else if (isAttacking) {
                isAttacking = false;
        }


        
        
        
        if(stateScript.CurrentState.ToString() == "EnemyMovingState" && !isAttacking){//MOVING STATE
            if(Vector3.Distance(this.gameObject.transform.position, currentPlayer.transform.position) <= 3) {
                anim.Play("FightStance");
            }else {
                anim.Play("Walk");
            }
        }else if(stateScript.CurrentState.ToString() == "EnemyHurtState" ){//HURT STATE


                if (stateScript.CurrentState.CurrentSubState.ToString() == "EnemyRecoveryState") {
                    anim.Play("Recover");
                }else if (stateScript._knockedDown){
                    if(stateScript.CurrentState.CurrentSubState.ToString() == "EnemyKnockedDownState") {//this makes the particle effect 
                        hitParticle.Play();
                    }
                    anim.Play("KnockedDown");
                    
                }else if (stateScript.CurrentState.CurrentSubState.ToString() == "EnemySmackedState") {
                    // anim.Play("Idle");
                    anim.Play("Hurt");
                }else {//add more code to account repeatedly getting hit
                    Debug.Log(stateScript.CurrentState.CurrentSubState.ToString());
                    // anim.Play("Idle");
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
