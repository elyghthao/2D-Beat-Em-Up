using System.Threading;
using UnityEngine;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyMovingState : EnemyBaseState {
   public EnemyMovingState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      Debug.Log("ENEMY ROOT: ENTERED MOVING");
      Ctx.Moving = true;
      Ctx.BaseMaterial.color = Color.magenta;
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      Debug.Log("ENEMY ROOT: EXITED MOVING");
      Ctx.Moving = false;
   }

   public override void CheckSwitchStates() {
      // Checking to see if the enemy got hurt
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
      }

      // Checking first if there is no player or the player is too far
      if (CurrentPlayerMachine == null) {
         SwitchState(Factory.Idle());
      } else {
         float dist = Vector3.Distance(Ctx.gameObject.transform.position, CurrentPlayerMachine.gameObject.transform.position);
         if (dist > Ctx.activationDistance) {
            SwitchState(Factory.Idle());
         }
      }
   }

   public override void InitializeSubState() {
      // If a heavy enemy, they will only ever chase
      if (Ctx.enemyType == EnemyStateMachine.EnemyType.Heavy) {
         SetSubState(Factory.Chase());
      }

      // Only state that should be set to the substate initially is the Stunned state
      // SetSubState(Factory.Stunned());
   }
}
