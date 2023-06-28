using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
   public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() { Debug.Log("ROOT: ENTERED ATTACK"); }

   public override void UpdateState() {
      if (CurrentSubState.CanSwitch) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("ROOT: EXITED ATTACK");
      Ctx.heavyAttackBounds.SetActive(false);
      Ctx.mediumAttackBounds.SetActive(false);
      Ctx.lightAttackBounds.SetActive(false);
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsMovementPressed) {
         SwitchState(Factory.Move());
      } else if(!Ctx.IsActionPressed || !Ctx.IsBlockHeld) {
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
