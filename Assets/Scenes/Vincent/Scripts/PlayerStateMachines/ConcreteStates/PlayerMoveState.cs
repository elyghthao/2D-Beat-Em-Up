using UnityEngine;

/// <summary>
/// Root state for the PlayerStateMachine
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
   public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() { 
      // Debug.Log("ROOT: ENTERED MOVEMENT"); 
      }

   public override void UpdateState() {
      Vector2 moveDir = Ctx.CurrentMovementInput * (Ctx.movementSpeed * 10f);
      // Applies movement to the player depending on the player input
      Ctx.Rigidbody.AddForce(new Vector3(moveDir.x, 0, moveDir.y), ForceMode.Force);
      Ctx.SpeedControl();
      CheckSwitchStates();
   }

   public override void ExitState() { 
      // Debug.Log("ROOT: EXITED MOVEMENT"); 
      }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         SwitchState(Factory.Attack());
      } else if (!Ctx.IsMovementPressed) {
         SwitchState(Factory.Idle());
      }
   }

   public override void InitializeSubState() {
      if (Ctx.CurrentMovementInput.x < 0) {
         SetSubState(Factory.Backward());
      } else if (Ctx.CurrentMovementInput.x > 0 || Ctx.CurrentMovementInput.y != 0) {
         SetSubState(Factory.Forward());
      }
   }
}
