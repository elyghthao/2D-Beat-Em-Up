using UnityEngine;

public class EnemyRecoveryState : EnemyBaseState {
   public EnemyRecoveryState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
      : base(currentContext, enemyStateFactory) { }

   public override void EnterState() {
      // Debug.Log("ENEMY ENTERING SUBSTATE: RECOVERY");
   }

   public override void UpdateState() {
      // Waiting for superstate to change states
   }

   public override void FixedUpdateState() {
      
   }

   public override void ExitState() {
      // Debug.Log("ENEMY EXITING SUBSTATE: RECOVERY");
   }

   public override void CheckSwitchStates() {
      throw new System.NotImplementedException();
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}