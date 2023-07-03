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
      if (Ctx.KnockdownMeter < Ctx.knockdownMax) {
         Debug.Log("Regenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter += Time.deltaTime * 50;
      } else if (Ctx.KnockdownMeter > Ctx.knockdownMax){
         Debug.Log("Degenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter = Ctx.knockdownMax;
      }
      // HaHa enemy do nothing (͡•͜ʖ͡•)
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED IDLE");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
