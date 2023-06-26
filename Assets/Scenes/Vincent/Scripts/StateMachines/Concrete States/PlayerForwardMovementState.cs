using UnityEngine;

public class PlayerForwardMovementState : PlayerBaseState
{
   public PlayerForwardMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }

   public override void EnterState() {
      Ctx.BaseMaterial.color = Color.cyan;
   }

   public override void UpdateState() {
      CheckSwitchStates();
      Vector2 moveDir = Ctx.CurrentMovementInput * (Ctx.movementSpeed * 10f);
      Ctx.Rigidbody.AddForce(new Vector3(moveDir.x, 0, moveDir.y), ForceMode.Force);
      Ctx.SpeedControl();
   }

   public override void ExitState() {
      Ctx.BaseMaterial.color = Color.white;
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
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
      } else if (!Ctx.IsMovementPressed) {
         SwitchState(Factory.Idle());
      } else if (Ctx.CurrentMovementInput.x < 0) {
         SwitchState(Factory.Backward());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
