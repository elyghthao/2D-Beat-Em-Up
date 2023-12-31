using System.Threading;
using UnityEngine;
using System.Collections;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyDeathState : EnemyBaseState {
   public EnemyDeathState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) { }

   public override void EnterState() {
      // Debug.Log("ENEMY ROOT: ENTERED DEATH STATE");
      if (Ctx.AgentObject) { MonoBehaviour.Destroy(Ctx.AgentObject); }
      Ctx.StartCoroutine(Ctx.DeathTimeDelay(2f));
   }

   public override void UpdateState() { 
      // Debug.Log("enemy is dead");
   }

   public override void FixedUpdateState() {
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED DEATH STATE");
   }

   public override void CheckSwitchStates() { }

   public override void InitializeSubState() { }
   
}


