using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockedDownState : PlayerBaseState
{
    public PlayerKnockedDownState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
        base(currentContext, playerStateFactory) {}
    public override void EnterState() {
        // Debug.Log("SUB: ENTERED KNOCKDOWN");
        Ctx.BaseMaterial.color = new Color(25, 0, 0, 255);
        // Sets the knockedDown bool in our context file to true, for other states to see
        Ctx.KnockedDown = true;
        Ctx.ApplyAttackStats();
        // Default stunTimer of 1.0 for knockdowns
        if (Ctx.StunTimer < 1.0f) {
            Ctx.StunTimer = 1.0f;
        }
    }

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void FixedUpdateState() {
        
    }

    public override void ExitState() {
        // Debug.Log("SUB: EXITED KNOCKDOWN");
        Ctx.KnockdownMeter = Ctx.knockdownMax;
    }

    public override void CheckSwitchStates() {
        SwitchState(Factory.Stunned());
    }

    public override void InitializeSubState() {
        throw new System.NotImplementedException();
    }
}
