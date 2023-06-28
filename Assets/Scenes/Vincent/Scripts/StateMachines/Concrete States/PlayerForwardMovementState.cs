using UnityEngine;

public class PlayerForwardMovementState : PlayerBaseState
{
   public PlayerForwardMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) { }

   public override void EnterState() {
      Debug.Log("SUB: ENTERED FORWARD");
      if (Ctx.CharacterFlipped) {
         Ctx.FlipCharacter();
      }
      Ctx.BaseMaterial.color = Color.cyan;
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      Debug.Log("SUB: EXITED FORWARD");
      Ctx.BaseMaterial.color = Color.white;
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
   }

   public override void CheckSwitchStates() {
      if (Ctx.CurrentMovementInput.x < 0) {
         SwitchState(Factory.Backward());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
