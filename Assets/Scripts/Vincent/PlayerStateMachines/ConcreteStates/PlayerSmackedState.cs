using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmackedState : PlayerBaseState
{
    public PlayerSmackedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
        base(currentContext, playerStateFactory) {}
    
    public override void EnterState() {
        Debug.Log("SUB: ENTERED SMACKED");
        if (!Ctx.KnockedDown) {
            Ctx.BaseMaterial.color = new Color(255, 68, 0, 255);
        }
        Ctx.ApplyAttackStats();
        // Sets the stun timer to 0.5f, which is the default for any non-knockdown attack
        if (Ctx.StunTimer < 0.5f) {
            Ctx.StunTimer = 0.5f;
        }
    }

    public override void UpdateState() {
        if (!Ctx.IsAttacked) {
            CheckSwitchStates();
        }
    }

    public override void ExitState() {
        Debug.Log("SUB: EXITED SMACKED");
    }

    public override void CheckSwitchStates() {
        SwitchState(Factory.Stunned());
    }

    public override void InitializeSubState() {
        throw new System.NotImplementedException();
    }
}
