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



    
    void Start()
    {
        stateScript = this.gameObject.GetComponent<PlayerStateMachine>();
        lightAttack = stateScript.lightAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
    }

    // Update is called once per frame
    void Update()
    {
        if(lightAttack.activeSelf){
            anim.Play("LightAttack");
        }else if(mediumAttack.activeSelf){
            anim.Play("MediumAttack");
        }else if(slamAttack.activeSelf){
            anim.Play("SlamAttack");
        }else if(stateScript.CurrentState.ToString() == "EnemyMovingState"){
            anim.Play("Walk");
        }else if(stateScript.CurrentState.ToString() == "EnemyHurtState"){
            anim.Play("Hurt");
        }else if(stateScript.CurrentState.ToString() == "EnemyIdleState"){
            anim.Play("Idle");
        }
        
        


        
        





        
    }
}
