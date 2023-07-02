using UnityEngine;

public class PlayerBlockState : PlayerBaseState {
   public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      CanSwitch = false;
   }
   
   public override void EnterState() {
      Debug.Log("SUB: ENTERED BLOCK");
      Ctx.BaseMaterial.color = Color.black;
   }
    
   public override void UpdateState() {
      if (!Ctx.IsBlockHeld) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("SUB: EXITED BLOCK");
      Ctx.BaseMaterial.color = Color.white;
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsLightAttackPressed) {
         SwitchState(Factory.LightAttack());
      } else if (Ctx.IsMediumAttackPressed) {
         SwitchState(Factory.MediumAttack());
      } else if (Ctx.IsHeavyAttackPressed) {
         SwitchState(Factory.HeavyAttack());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
