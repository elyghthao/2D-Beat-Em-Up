using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
   public EnemyStunnedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED STUNNED");
      // Sets material color Orange
      if (!Ctx.KnockedDown) {
         Ctx.BaseMaterial.color = new Color(255, 165, 0);
      }
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      Debug.Log("ENEMY SUB: EXITED STUNNED");
   }

   public override void CheckSwitchStates() {
      if (Ctx.IsAttacked) {
         if (Ctx.KnockdownMeter > 0) {
            SwitchState(Factory.Smacked());
         } else if (!Ctx.KnockedDown) {
            SwitchState(Factory.KnockedDown());
         }
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
