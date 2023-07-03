using System.Data.Common;
using UnityEngine;

public class EnemyKnockedDownState : EnemyBaseState {
   public EnemyKnockedDownState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED KNOCKDOWN");
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
      Ctx.Rigidbody.AddForce(5, 500, 0);
      Ctx.BaseMaterial.color = new Color(25, 0, 0, 255);
      Ctx.KnockedDown = true;
      Ctx.KnockdownMeter -= Ctx.DetermineKnockdownPressure();
      if (Ctx.StunTimer < 1.0f) {
         Ctx.StunTimer = 1.0f;
      }
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      Debug.Log("ENEMY SUB: EXITED KNOCKDOWN");
      Ctx.KnockdownMeter = Ctx.knockdownMax;
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Stunned());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
