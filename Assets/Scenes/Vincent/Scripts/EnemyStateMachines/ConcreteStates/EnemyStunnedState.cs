using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
   public EnemyStunnedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      
   }

   public override void EnterState() {
      throw new System.NotImplementedException();
   }

   public override void UpdateState() {
      throw new System.NotImplementedException();
   }

   public override void ExitState() {
      throw new System.NotImplementedException();
   }

   public override void CheckSwitchStates() {
      throw new System.NotImplementedException();
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
