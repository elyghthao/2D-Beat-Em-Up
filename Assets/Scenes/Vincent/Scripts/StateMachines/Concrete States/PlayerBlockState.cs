using UnityEngine;

public class PlayerBlockState : PlayerBaseState {
   private bool _finishedAnimation = false;
   public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }
   
   public override void EnterState() {
      Ctx.BaseMaterial.color = Color.black;
   }
    
   public override void UpdateState() {
      if (!Ctx.IsBlockHeld || (Ctx.IsActionPressed && !Ctx.IsBlockPressed)) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Ctx.BaseMaterial.color = Color.white;
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         if (Ctx.IsLightAttackPressed) {
            SwitchState(Factory.LightAttack());
         } else if (Ctx.IsMediumAttackPressed) {
            SwitchState(Factory.MediumAttack());
         } else if (Ctx.IsHeavyAttackPressed) {
            SwitchState(Factory.HeavyAttack());
         }
      } else if (Ctx.IsMovementPressed) {
         if (Ctx.CurrentMovementInput.x < 0) {
            SwitchState(Factory.Backward());
         } else if (Ctx.CurrentMovementInput.x > 0) {
            SwitchState(Factory.Forward());
         }
      }  else {
         SwitchState(Factory.Idle());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
