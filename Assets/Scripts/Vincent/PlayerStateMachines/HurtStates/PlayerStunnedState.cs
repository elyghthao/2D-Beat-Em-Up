using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : PlayerBaseState {
    public PlayerStunnedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
        base(currentContext, playerStateFactory) {}
    private bool _wentAirborne;
    public override void EnterState() {
        // Debug.Log("SUB: ENTERED STUNNED");
        // Sets material color Orange
        if (!Ctx.KnockedDown) {
            Ctx.BaseMaterial.color = new Color(255, 165, 0);
        }
    }

    public override void UpdateState() {
        if (!Ctx.IsGrounded) {
         _wentAirborne = true;
      }
      CheckSwitchStates();
      if (Ctx.IsGrounded) {
         _wentAirborne = false;
      }
    }

    public override void FixedUpdateState() {
        
    }

    public override void ExitState() {
        // Debug.Log("SUB: EXITED STUNNED");
    }

    public override void CheckSwitchStates() {
        // If we've been attacked, check to see if we should be knocked down or not.
        // Debug.Log("isGrounded: " + Ctx.IsGrounded + "       KnockedDown: " + Ctx.KnockedDown + "        wentAirborne: " + _wentAirborne);
        if (Ctx.IsGrounded && Ctx.KnockedDown && _wentAirborne) {
         SwitchState(Factory.Recover());
         return;
        }
        if (Ctx.IsAttacked) {
            if (Ctx.KnockdownMeter > 0) {
                SwitchState(Factory.Smacked());
            } else if (!Ctx.KnockedDown) {
                SwitchState(Factory.KnockedDown());
            }
        }
        // Otherwise, stay in stunned until our root state switches us to a different state
    }

    public override void InitializeSubState() {
        throw new System.NotImplementedException();
    }
}
