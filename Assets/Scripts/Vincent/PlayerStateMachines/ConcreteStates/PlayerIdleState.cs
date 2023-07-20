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
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
      } else if (Ctx.IsActionPressed) {
         // Pathway: If we pressed the powerup key and we have an attack powerup, we attack.
         // If we pressed the powerup key and we do not have an attack powerup equipped, dont do anything.
         // If we did not press a powerup key, then just attack.
         if (Ctx.IsPowerupPressed) {
            if (Ctx.PowerupSystem.attackEquipped()) {
               SwitchState(Factory.Attack());
            }
         } else {
            SwitchState(Factory.Attack());
         }
      } else if (Ctx.IsMovementPressed) {
         SwitchState(Factory.Move());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
