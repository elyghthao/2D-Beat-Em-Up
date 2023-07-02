using System.Data.Common;
using UnityEngine;

public class EnemyKnockedDownState : EnemyBaseState {
   private float _knockdownTimer = 1f;
   private float _curTimer = 0;
   public EnemyKnockedDownState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   }

   public override void EnterState() {
      Debug.Log("ENEMY ROOT: ENTERED KNOCKDOWN");
      Ctx.Rigidbody.AddForce(5, 500, 0);
      Ctx.BaseMaterial.color = new Color(50, 0, 0, 255);
   }

   public override void UpdateState() {
      _curTimer += Time.deltaTime;
      if (_curTimer >= _knockdownTimer) {
         CheckSwitchStates();
      }
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED KNOCKDOWN");
      Ctx.KnockdownMeter = Ctx.knockdownMax;
   }

   public override void CheckSwitchStates() {
      SwitchState(Factory.Idle());
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
