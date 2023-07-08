using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
    public EnemyStateMachine stateScript;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        stateScript = this.gameObject.GetComponent<EnemyStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(stateScript.CurrentState.ToString());
        if(stateScript.CurrentState.ToString() == "EnemyMovingState"){
            anim.Play("Walk");
        }else if(stateScript.CurrentState.ToString() == "EnemyIdleState"){
            anim.Play("Idle");
        }
    }
}
