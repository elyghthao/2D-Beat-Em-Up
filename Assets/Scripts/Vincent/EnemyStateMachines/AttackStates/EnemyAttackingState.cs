using System.Threading;
using UnityEngine;

/// <summary>
/// Root state for when the enemy is hurt
/// </summary>
public class EnemyAttackingState : EnemyBaseState {
   public EnemyAttackingState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
      IsRootState = true;
      InitializeSubState();
   }

   public override void EnterState() {
      // Debug.Log("ENEMY ROOT: ENTERED ATTACKING");
      Ctx.Attacking = true;
      Ctx.BaseMaterial.color = Color.red;
      Ctx.Rigidbody.velocity = Vector3.zero;
      Ctx.LastAttacked = Time.deltaTime;
   }

   public override void UpdateState() {
      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED MOVING");
      Ctx.Attacking = false;
   }

   public override void CheckSwitchStates() {
      // Checking to see if the enemy got hurt
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
      }

      // Checking only if we are not attacking anymore (that is turned off in substates unless this state is forced out)
      if (!Ctx.Attacking) {
         // Checking first if there is no player or the player is too far
         if (CurrentPlayerMachine == null) {
            SwitchState(Factory.Idle());
         } else {
            float dist = Vector3.Distance(Ctx.gameObject.transform.position, CurrentPlayerMachine.gameObject.transform.position); // 
            if (dist > Ctx.attackDistance) { // too far, go back to idle
               SwitchState(Factory.Idle());
            } 
         }
      }
   }

   public override void InitializeSubState() {
      // If a heavy enemy, can do heavy and medium attacks
      if (Ctx.enemyType == EnemyStateMachine.EnemyType.Heavy) {
         int attackNumber = Random.Range(1, 3); // 1 for heavy, 2 for medium
         if (attackNumber == 1) {
            SetSubState(Factory.HeavyAttack());
         } else if (attackNumber == 2) {
            SetSubState(Factory.MediumAttack());
         }
      }
   }
}
