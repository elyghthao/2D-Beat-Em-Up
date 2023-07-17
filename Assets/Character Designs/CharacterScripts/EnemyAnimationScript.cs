using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
    public EnemyStateMachine stateScript;
    public Animator anim;
    public GameObject lightAttack;
    public GameObject mediumAttack;
    public GameObject slamAttack;

    // Start is called before the first frame update
    void Start()
    {
        stateScript = this.gameObject.GetComponent<EnemyStateMachine>();
        lightAttack = stateScript.lightAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(mediumAttack.activeSelf);
        // Debug.Log(stateScript.CurrentState.ToString());


        if(lightAttack.activeSelf){
            anim.Play("LightAttack");
        }else if(mediumAttack.activeSelf){
            anim.Play("MediumAttack");
        }else if(stateScript.CurrentState.ToString() == "EnemyMovingState"){
            anim.Play("Walk");
        }else if(stateScript.CurrentState.ToString() == "EnemyIdleState"){
            anim.Play("Idle");
        }else if(stateScript.CurrentState.ToString() == "EnemyHurtState"){
            anim.Play("Hurt");
        }
    }
}
