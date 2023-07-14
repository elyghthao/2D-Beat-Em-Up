using UnityEngine;

/// <summary>
/// Substate of EnemyHurtState. For when the enemy is smacked by any attack that hasn't immediately knocked them down.
/// </summary>
public class EnemySmackedState : EnemyBaseState
{
   public EnemySmackedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED SMACKED");
      if (!Ctx.KnockedDown) {
         Ctx.BaseMaterial.color = Color.red;
      }
      // Sets the velocity to 0 so there's no countering velocity when trying to apply the knockback
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
      // Default knockback settings, should be updated depending on the type of attack applied
      Ctx.Rigidbody.AddForce(5f, 200, 0);
      Ctx.KnockdownMeter -= Ctx.DetermineKnockdownPressure();
      // Sets the stun timer to 0.5f, which is the default for any non-knockdown attack
      if (Ctx.StunTimer < 0.5f) {
         Ctx.StunTimer = 0.5f;
      }
   }

   public override void UpdateState() {
      if (!Ctx.IsAttacked) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("ENEMY SUB: EXITED SMACKED");
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Stunned());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
