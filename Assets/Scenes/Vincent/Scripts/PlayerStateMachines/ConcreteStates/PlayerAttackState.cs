using UnityEngine;

/// <summary>
/// Root state, when the player is doing an attack or blocking
/// </summary>
public class PlayerAttackState : PlayerBaseState
{
   public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      //Debug.Log("ROOT: ENTERED ATTACK");
   }

   public override void UpdateState() {
      // Will ONLY switch if the current substate is ready to me switched out of
      if (CurrentSubState.CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      //Debug.Log("ROOT: EXITED ATTACK");
      // Checks that all bounds are deactivated, as a failsafe so no bounds are left on when leaving the attack state
      Ctx.heavyAttackBounds.SetActive(false);
      Ctx.mediumAttackBounds.SetActive(false);
      Ctx.lightAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsMovementPressed) {
         SwitchState(Factory.Move());
      } else if(!Ctx.IsActionHeld) {
         SwitchState(Factory.Idle());
      }
   }

   public override void InitializeSubState() {
      if (Ctx.IsLightAttackPressed) {
         SetSubState(Factory.LightAttack());
      } else if (Ctx.IsMediumAttackPressed) {
         SetSubState(Factory.MediumAttack());
      } else if (Ctx.IsHeavyAttackPressed) {
         SetSubState(Factory.HeavyAttack());
      } else if (Ctx.IsBlockPressed) {
         SetSubState(Factory.Block());
      }
   }
}
