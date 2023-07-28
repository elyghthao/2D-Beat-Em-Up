using System.Threading;
using UnityEngine;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyDeathState : EnemyBaseState {
   public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) { }

   public override void EnterState() {
      Ctx.SetDead();
   }

   public override void UpdateState() { }

   public override void ExitState() { }

   public override void CheckSwitchStates() { }

   public override void InitializeSubState() { }
}
