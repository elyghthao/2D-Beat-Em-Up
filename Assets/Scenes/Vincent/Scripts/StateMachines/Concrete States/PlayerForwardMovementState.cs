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
      Debug.Log("Entering Forward state");
   }

   public override void UpdateState() {
      CheckSwitchStates();
      Vector2 moveDir = Ctx.CurrentMovementInput * (Ctx.movementSpeed * 10f);
      Ctx.Rigidbody.AddForce(new Vector3(moveDir.x, 0, moveDir.y), ForceMode.Force);
   }

   public override void ExitState() {
      Debug.Log("Exiting Forward state");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         // Implement doing action
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
