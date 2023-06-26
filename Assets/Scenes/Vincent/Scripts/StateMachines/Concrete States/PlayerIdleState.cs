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
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         if (Ctx.IsLightAttackPressed) {
            SwitchState(Factory.LightAttack());
         } else if (Ctx.IsMediumAttackPressed) {
            SwitchState(Factory.MediumAttack());
         } else if (Ctx.IsHeavyAttackPressed) {
            SwitchState(Factory.HeavyAttack());
         } else if (Ctx.IsBlockPressed) {
            SwitchState(Factory.Block());
         }
      } else if (Ctx.IsMovementPressed) {
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
