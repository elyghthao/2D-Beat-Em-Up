using UnityEngine;

public class EnemyIdleState : EnemyBaseState {
   public EnemyIdleState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
   }

   public override void EnterState() {
      Debug.Log("ENEMY ROOT: ENTERED IDLE");
      Ctx.BaseMaterial.color = Color.green;
   }

   public override void UpdateState() {
      CheckSwitchStates();
      // HaHa enemy do nothing (͡•͜ʖ͡•)
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED IDLE");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsAttacked) {
         if (Ctx.KnockdownMeter <= 0) {
            SwitchState(Factory.KnockedDown());
         } else {
            SwitchState(Factory.Hurt());
         }
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
