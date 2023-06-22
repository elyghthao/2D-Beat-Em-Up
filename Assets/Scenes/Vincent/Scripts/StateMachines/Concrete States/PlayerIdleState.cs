using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
   public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
      : base(currentContext, playerStateFactory) { }

   public override void EnterState() {
      throw new System.NotImplementedException();
   }

   public override void UpdateState() {
      if (Ctx.IsMovementPressed) {
         if (Ctx.CurrentMovementInput.x < 0) {
            SwitchState(Factory.Backward());
         }
      }
   }

   public override void ExitState() {
      throw new System.NotImplementedException();
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsMovementPressed && !Ctx.IsActionPressed) {
         if (Ctx.CurrentMovementInput.x < 0) {
            SwitchState(Factory.Backward());
         }
         if (Ctx.CurrentMovementInput.x > 0) {
            SwitchState(Factory.Forward());
         }
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
