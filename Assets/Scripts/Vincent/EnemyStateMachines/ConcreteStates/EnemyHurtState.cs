using System.Threading;
using UnityEngine;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyHurtState : EnemyBaseState {
   public EnemyHurtState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      Debug.Log("ENEMY ROOT: ENTERED HURT");
   }

   public override void UpdateState() {
      // If the stun timer has reached 0 or less, then we can transition out of being hurt. Substates will update the 
      // stun timer accordingly
      if (Ctx.IsGrounded) {
         Ctx.StunTimer -= Time.deltaTime;
      }

      if (Ctx.StunTimer <= 0 && CurrentSubState.ToString() != "EnemyDeathState") {
         CheckSwitchStates();
      }
   }

   public override void FixedUpdateState() {
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED HURT");
      Ctx.KnockedDown = false;
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      // Only state that should be set to the substate initially is the Stunned state
      SetSubState(Factory.Stunned());
   }
}
