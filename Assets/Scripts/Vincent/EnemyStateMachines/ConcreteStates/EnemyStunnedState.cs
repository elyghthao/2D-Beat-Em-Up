using UnityEngine;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyStunnedState : EnemyBaseState {
   private bool _wentAirborne;
   public EnemyStunnedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   public override void EnterState() {
      // Debug.Log("ENEMY SUB: ENTERED STUNNED");
      // Sets material color Orange
      if (!Ctx.KnockedDown) {
         Ctx.BaseMaterial.color = new Color(255, 165, 0);
      }
   }

   public override void UpdateState() {
      if (!Ctx.IsGrounded) {
         _wentAirborne = true;
      }
      CheckSwitchStates();
      if (Ctx.IsGrounded) {
         _wentAirborne = false;
      }
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
      //Debug.Log("ENEMY SUB: EXITED STUNNED");
   }

   public override void CheckSwitchStates() {
      // If we've been attacked, check to see if we should be knocked down or not.
      // Debug.Log("isgrounded: " + Ctx.IsGrounded + "     knockeddown: " + Ctx.KnockedDown + "     wentAirborne: " + _wentAirborne);
      if (Ctx.IsGrounded && Ctx.KnockedDown && _wentAirborne) {
         SwitchState(Factory.Recovery());
         return;
      }
      if (Ctx.IsAttacked) {
         if (Ctx.KnockdownMeter > 0) {
            SwitchState(Factory.Smacked());
         } else if (!Ctx.KnockedDown) {
            SwitchState(Factory.KnockedDown());
         }
      }

      
      // Otherwise, stay in stunned until our root state switches us to a different state
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}