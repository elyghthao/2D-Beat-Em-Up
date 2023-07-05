using System;
using UnityEngine;

/// <summary>
/// Default state for the player, when nothing is happening to or from the player
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
   public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) {
      IsRootState = true;
      //InitializeSubState();
   }

   public override void EnterState() {

      Ctx.BaseMaterial.color = Color.green;
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsActionPressed) {
         SwitchState(Factory.Attack());
      } else if (Ctx.IsMovementPressed) {
         SwitchState(Factory.Move());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
