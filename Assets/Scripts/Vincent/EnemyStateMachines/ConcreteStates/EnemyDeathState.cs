using System.Threading;
using UnityEngine;
using System.Collections;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyDeathState : EnemyBaseState {
   public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) { }

   public override void EnterState() {
      // Ctx.SetDead();
      Ctx.StartCoroutine(Ctx.DeathTimeDelay(1f));
   }

   public override void UpdateState() { 
      // Debug.Log("enemy is dead");
   }

   public override void FixedUpdateState() {
   }

   public override void ExitState() { }

   public override void CheckSwitchStates() { }

   public override void InitializeSubState() { }
   
}


