using System.Data.Common;
using UnityEngine;

/// <summary>
/// Substate of the EnemyKnockedDownstate. for when the knockdown meter has been depleted
/// </summary>
public class EnemyKnockedDownState : EnemyBaseState {
   public EnemyKnockedDownState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED KNOCKDOWN");
      // Sets the velocity to 0 so there's no countering velocity when trying to apply the knockback
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
      // Default knockback for knockdowns
      Ctx.Rigidbody.AddForce(5, 500, 0);
      Ctx.BaseMaterial.color = new Color(25, 0, 0, 255);
      // Sets the knockedDown bool in our context file to true, for other states to see
      Ctx.KnockedDown = true;
      Ctx.KnockdownMeter -= Ctx.GetPressureAndDamage();
      // Default stunTimer of 1.0 for knockdowns
      if (Ctx.StunTimer < 1.0f) {
         Ctx.StunTimer = 1.0f;
      }
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED KNOCKDOWN");
      Ctx.KnockdownMeter = Ctx.knockdownMax;
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Stunned());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
