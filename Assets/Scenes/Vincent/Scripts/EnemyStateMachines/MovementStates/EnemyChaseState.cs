using UnityEngine;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
   public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   public override void EnterState() {
      Debug.Log("ENEMY SUB: ENTERED CHASE");
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      Debug.Log("ENEMY SUB: EXITED CHASE");
   }

   public override void CheckSwitchStates() {
      
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}
