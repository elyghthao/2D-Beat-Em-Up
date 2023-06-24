using System;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
   public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }

   public override void EnterState() {
      Ctx.BaseMaterial.color = Color.green;
      Debug.Log("Entering Idle state");
   }

   public override void UpdateState() {
      CheckSwitchStates();
      if (Ctx.IsMovementPressed) {
         if (Ctx.CurrentMovementInput.x < 0) {
            SwitchState(Factory.Backward());
         }
      }
   }

   public override void ExitState() {
      Debug.Log("Exiting Idle state");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsMovementPressed && !Ctx.IsActionPressed) {
         if (Ctx.CurrentMovementInput.x < 0) {
            SwitchState(Factory.Backward());
         } else if (Ctx.CurrentMovementInput.x > 0 || Ctx.CurrentMovementInput.y != 0) {
            SwitchState(Factory.Forward());
         }
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
