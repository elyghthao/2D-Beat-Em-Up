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
                if (rand == 0){
                    Debug.Log("light attack");
                    anim.Play("LightAttack");
                }else {
                    Debug.Log("Light attack 1");
                    anim.Play("LightAttack1");
                }
        }else if(mediumAttack.activeSelf){
            anim.Play("MediumAttack");
        }else if(slamAttack.activeSelf){
            anim.Play("SlamAttack");
        }
        }else if (isAttacking) {
            isAttacking = false;
            rand = Random.Range(0, 2);
        }


        
        
        
        if(stateScript.CurrentState.ToString() == "PlayerMoveState" && !isAttacking){
            isHit = false;
            anim.Play("Walk");
        }else if(stateScript.CurrentState.ToString() == "PlayerHurtState" ){
            anim.Play("Hurt");
            if(!isHit){
                anim.Play("Hurt");
                hitParticle.Play();
                isHit = true;
            }
        }else if(stateScript.CurrentState.ToString() == "PlayerIdleState" && !isAttacking){
            isHit = false;
            anim.Play("Idle");
        }
        
        


        
        





        
    }

    IEnumerator RandNum() {
        Debug.Log("Running");
        rand = Random.Range(0, 2);
        yield return new WaitForSeconds(.5f);
        // StartCoroutine(RandNum());
    }
}
