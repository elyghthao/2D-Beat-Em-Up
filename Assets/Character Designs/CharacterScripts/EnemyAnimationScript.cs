using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
    public EnemyStateMachine stateScript;
    public Animator anim;
    public GameObject lightAttack;
    public GameObject mediumAttack;
    public GameObject slamAttack;
    private int rand ;
    public bool isAttacking;
    public bool isHit;
    public ParticleSystem hitParticle;
    private bool _ready;
    
    // Start is called before the first frame update
    void Start()
    {
        stateScript = this.gameObject.GetComponent<EnemyStateMachine>();
        StartCoroutine(checkStateReady());
        lightAttack = stateScript.lightAttackBounds;
        mediumAttack = stateScript.mediumAttackBounds;
        slamAttack = stateScript.heavyAttackBounds;
        isAttacking = false;
        isHit = false;
        
    }

    // Update is called once per frame
    void Update() {
        if (!_ready) return;
        // Debug.Log(mediumAttack.activeSelf);
        Debug.Log(stateScript.CurrentState.ToString());

        if(stateScript.CurrentState.ToString() == "EnemyAttackingState") {
            isAttacking = true;
            isHit = false;
            if(lightAttack.activeSelf){
                anim.Play("LightAttack");
            }else if(mediumAttack.activeSelf){
                anim.Play("MediumAttack");
            }else if(slamAttack.activeSelf){
                anim.Play("SlamAttack");
            }
            }else if (isAttacking) {
                isAttacking = false;
                rand = Random.Range(0, 2);
        }


        
        
        
        if(stateScript.CurrentState.ToString() == "EnemyMovingState" && !isAttacking){
            anim.Play("Walk");
            isHit = false;
        }else if(stateScript.CurrentState.ToString() == "EnemyHurtState" ){
            if(!isHit){
                anim.Play("Hurt");
                hitParticle.Play();
                isHit = true;
            }
            
        }else if(stateScript.CurrentState.ToString() == "EnemyIdleState" && !isAttacking){
            isHit = false;
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
