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
      // Debug.Log("ENEMY ROOT: ENTERED MOVING");
      Ctx.Moving = true;
      Ctx.BaseMaterial.color = Color.magenta;
   }

   public override void UpdateState() {
      // Chasing goal Transform and offsets
      Vector3 goalPos = Ctx.MovingGoal.position; // NOTE: The y for the MovingGoalOffset is really the z
      goalPos.x += Ctx.MovingGoalOffset.x;
      goalPos.y = Ctx.gameObject.transform.position.y;
      goalPos.z += Ctx.MovingGoalOffset.y;

      // Chasing the goal with the offset
      Vector3 directionToGoal = goalPos - Ctx.gameObject.transform.position;
      directionToGoal = directionToGoal.normalized * Ctx.movementSpeed * 10f;
      // Debug.Log("Postion:" + Ctx.CurrentPlayerMachine.gameObject.transform.position);
      // if(Vector3.Distance(Ctx.gameObject.transform.position, Ctx.CurrentPlayerMachine.gameObject.transform.position) > 2){
      //    Ctx.Rigidbody.AddForce(directionToGoal, ForceMode.Force);
      // }else {
      // }

      Ctx.Rigidbody.AddForce(directionToGoal, ForceMode.Force);
      
      Ctx.SpeedControl();

      // Make it so the right of enemy will always face the Transform goal when chasing
      Vector3 enemyScale = Ctx.transform.localScale;
      if(directionToGoal.x > 0) {
         // Ctx.transform.localEulerAngles = new Vector3(0, 0, 0);
         Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      }else {
         // Ctx.transform.localEulerAngles = new Vector3(0, -180, 0);
         Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      }

      // Update KnowckdownMeter
      if (Ctx.KnockdownMeter < Ctx.knockdownMax) {
         // Debug.Log("Regenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter += Time.deltaTime * 50;
      } else if (Ctx.KnockdownMeter > Ctx.knockdownMax){
         // Debug.Log("Degenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter = Ctx.knockdownMax;
      }
      
      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED MOVING");
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
         if (dist > Ctx.activationDistance) { // too far, go back to idle
            SwitchState(Factory.Idle());
         } else if (dist <= Ctx.attackDistance) {
            // Close enough to attack, but now checking if enough time elapsed to allow us to attack
            if (Ctx.LastAttacked >= Ctx.attackReliefTime) {
               SwitchState(Factory.Attack());
            } else {
               Ctx.LastAttacked += Time.deltaTime;
            }
         }
      }
   }

   public override void InitializeSubState() {
      // If a heavy enemy, they will only ever chase
      if (Ctx.enemyType == EnemyStateMachine.EnemyType.Heavy) {
         SetSubState(Factory.Chase());
      } else if (Ctx.enemyType == EnemyStateMachine.EnemyType.Medium) {
         SetSubState(Factory.Chase());
      }else if (Ctx.enemyType == EnemyStateMachine.EnemyType.Light) {
         SetSubState(Factory.Chase());
      }

      // Only state that should be set to the substate initially is the Stunned state
      // SetSubState(Factory.Stunned());
   }
}


/*
TEST CODE FOR UPDATE

Vector2 movementDir = new Vector2(1, 0);
      Vector3 directionToPlayer = Ctx.CurrentPlayerMachine.gameObject.transform.position - Ctx.gameObject.transform.position;
      if (directionToPlayer.x < 0) {
         movementDir = new Vector2(-1, 0);
      }
      Vector2 moveDir = movementDir * (Ctx.movementSpeed * 10f);
      // Applies movement to the player depending on the player input
      Ctx.Rigidbody.AddForce(new Vector3(moveDir.x, 0, moveDir.y), ForceMode.Force);
      Ctx.SpeedControl();

      // make it so the right of enemy will always face player when chasing
      Vector3 enemyScale = Ctx.transform.localScale;
      if(directionToPlayer.x > 0) {
         Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x),enemyScale.y,enemyScale.z);
      }else {
         Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x),enemyScale.y,enemyScale.z);
      }
*/