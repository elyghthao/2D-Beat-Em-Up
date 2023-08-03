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
      Vector2 moveDir = Ctx.CurrentMovementInput * (Ctx.movementSpeed * 10f);
      // Applies movement to the player depending on the player input
      Ctx.Rigidbody.velocity = new Vector3(moveDir.x, 0, moveDir.y);
      Ctx.SpeedControl();
      Ctx.StaminaRegenAllowed = true;
   }

   public override void UpdateState() {
      Vector2 moveDir = Ctx.CurrentMovementInput * (Ctx.movementSpeed * 10f);
      // Applies movement to the player depending on the player input
      Ctx.GetComponent<Rigidbody>().velocity = new Vector3(moveDir.x, 0, moveDir.y);
      Ctx.SpeedControl();
      // Debug.Log(Ctx);
      CheckSwitchStates();
   }

   public override void ExitState() { 
      // Debug.Log("ROOT: EXITED MOVEMENT"); 
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
      } else  if (Ctx.IsActionPressed) {
         // Pathway: If we pressed the powerup key and we have an attack powerup, we attack.
         // If we pressed the powerup key and we do not have an attack powerup equipped, dont do anything.
         // If we did not press a powerup key, then just attack.
         if (Ctx.IsPowerupPressed) {
            if (Ctx.PowerupSystem.AttackEquipped()) {
               SwitchState(Factory.Attack());
            }
         } else {
            SwitchState(Factory.Attack());
         }
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
