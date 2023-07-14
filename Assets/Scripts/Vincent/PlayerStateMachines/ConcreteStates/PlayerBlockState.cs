using UnityEngine;

/// <summary>
/// Substate of the playerAttackState, when the player is blocking
/// </summary>
public class PlayerBlockState : PlayerBaseState {
   public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      // Set canSwitch to false so we can constrain when it's ok to switch from this state
      CanSwitch = false;
   }
   
   public override void EnterState() {
      // Debug.Log("SUB: ENTERED BLOCK");
      Ctx.BaseMaterial.color = Color.black;
   }
    
   public override void UpdateState() {
      // Only checks to switch state if block is currently being held
      if (!Ctx.IsBlockHeld) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED BLOCK");
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