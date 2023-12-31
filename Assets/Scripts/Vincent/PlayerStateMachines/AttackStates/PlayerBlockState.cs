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
      Ctx.MostRecentAttack = ToString();
      Ctx.StaminaRegenAllowed = true;
      Ctx.KnockdownMeter = Ctx.knockdownMax;//this is experimental, added to reduce fustration of being knockedback -elygh
   }
    
   public override void UpdateState() {
      // Only checks to switch state if block is currently being held
      if (!Ctx.IsBlockHeld) {
         CanSwitch = true;
         CheckSwitchStates();
      }
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
      // Debug.Log("SUB: EXITED BLOCK");
      Ctx.BaseMaterial.color = Color.white;
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Idle(), true); // TEMP FIX for action not ending because the action is being held down
      
      // if (Ctx.IsLightAttackPressed) {
      //    SwitchState(Factory.LightAttack());
      // } else if (Ctx.IsMediumAttackPressed) {
      //    SwitchState(Factory.MediumAttack());
      // } else if (Ctx.IsPowerupPressed) {
      //    if (Ctx.PowerupSystem.IsEquipped(PowerupSystem.Powerup.Slam)) {
      //       SetSubState(Factory.HeavyAttack());
      //    } else if (Ctx.PowerupSystem.IsEquipped(PowerupSystem.Powerup.Dash)) {
      //       SetSubState(Factory.DashAttack());
      //    }
      // }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
