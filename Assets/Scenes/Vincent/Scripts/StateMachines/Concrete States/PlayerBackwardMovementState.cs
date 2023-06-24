using UnityEngine;

public class PlayerBackwardMovementState : PlayerBaseState
{
   public PlayerBackwardMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }

   public override void EnterState() {
      Ctx.BaseMaterial.color = Color.blue;
      Debug.Log("Entering Backward state");
   }

   public override void UpdateState() {
      CheckSwitchStates();
      Vector2 moveDir = Ctx.CurrentMovementInput * (Ctx.movementSpeed * 10f);
      Ctx.Rigidbody.AddForce(new Vector3(moveDir.x, 0, moveDir.y), ForceMode.Force);
      Ctx.SpeedControl();
   }

   public override void ExitState() {
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
      Debug.Log("Exiting Backward state");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         // Implement doing action
      } else if (!Ctx.IsMovementPressed) {
         SwitchState(Factory.Idle());
      } else if (Ctx.CurrentMovementInput.x > 0) {
         SwitchState(Factory.Forward());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
