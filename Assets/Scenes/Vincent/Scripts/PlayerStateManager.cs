using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
    public PlayerBaseState currentState;
    
    public PlayerLAttackState lightAttack = new PlayerLAttackState();
    public PlayerMAttackState mediumAttack = new PlayerMAttackState();
    public PlayerHAttackState heavyAttack = new PlayerHAttackState();

    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerBlockState blockState = new PlayerBlockState();
    public PlayerHurtState hurtState = new PlayerHurtState();
    
    // Start is called before the first frame update
    void Start() {
        currentState = idleState;
        
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision) {
        currentState.OnCollisionEnter(this, collision);
    }

    public void SwitchState(PlayerBaseState state) {
        currentState = state;
        state.EnterState(this);
    }
}
