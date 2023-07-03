using UnityEngine;

public class EnemySmackedState : EnemyBaseState
{
   public EnemySmackedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED SMACKED");
      if (Ctx.KnockedDown) {
         Ctx.BaseMaterial.color = Color.red;
      }
      Ctx.Rigidbody.velocity = new Vector3(0, 0, 0);
      Ctx.Rigidbody.AddForce(5f, 200, 0);
      Ctx.KnockdownMeter -= Ctx.DetermineKnockdownPressure();
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
