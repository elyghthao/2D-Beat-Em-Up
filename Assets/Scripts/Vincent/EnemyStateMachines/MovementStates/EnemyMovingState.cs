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
      if (Ctx.MovingGoal == null) { Ctx.MovingGoal = Ctx.CurrentPlayerMachine.transform; }

      // Information
      Vector3 dirFaceVec = Ctx.MovingGoal.position - Ctx.gameObject.transform.position; 
      Vector3 goalOffset = new Vector3(dirFaceVec.x, dirFaceVec.y, dirFaceVec.z);
      Rigidbody PlayerRigidBody = Ctx.CurrentPlayerMachine.Rigidbody;
      float distanceToGoal = Vector3.Distance(Ctx.gameObject.transform.position, Ctx.MovingGoal.position);

      // AI navigation
      if (Ctx.HasAgent) {
         // Changing direction based off of the direction the agent is moving
         dirFaceVec = Ctx.RealAgent.velocity;

         // Getting the goal position
         Vector3 goalPos = Ctx.MovingGoal.position; // NOTE: The y for the MovingGoalOffset is really the z
         goalPos.x += Ctx.MovingGoalOffset.x;
         goalPos.y = Ctx.gameObject.transform.position.y;
         goalPos.z += Ctx.MovingGoalOffset.y;

         // Checking to see if the enemy is within distanceGoal
         distanceToGoal = Vector3.Distance(Ctx.gameObject.transform.position, goalPos);
         if ((distanceToGoal <= Ctx.distanceGoal) && (dirFaceVec.magnitude <= 3.5f)) {
            Ctx.inPosition = true;
         } else {
            Ctx.inPosition = false;
         }

         // Moving agent to that goal position
         Ctx.RealAgent.SetDestination(goalPos);

         // Moving enemy to the agent
         Vector3 newPos = Ctx.AgentObject.transform.position;
         newPos.y = Ctx.gameObject.transform.position.y;
         Ctx.gameObject.transform.position = newPos;
      } 

      // Checking where the enemy is compared to the player for smarter directional behavior
      bool flip = false;
      if (Ctx.EnemyFlankType == EnemyStateMachine.FlankType.Left) {
         if ((goalOffset.x >= 1) && (Mathf.Abs(goalOffset.z) < 2.3f) && (goalOffset.x < 8.3f)) {
            flip = false;
         } else {
            flip = !(dirFaceVec.x >= -.35f);
         }
      } else {
         if ((goalOffset.x <= -1) && (Mathf.Abs(goalOffset.z) < 2.3f) && (goalOffset.x > -8.3f)) {
            flip = true;
         } else {
            flip = !(dirFaceVec.x >= .35f);
         }
      }

      // Flipping if needed
      Vector3 enemyScale = Ctx.transform.localScale;
      if (!flip) {
         Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      } else {
         Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      }

      CheckSwitchStates();
   }

   public override void FixedUpdateState() {
      // Update KnowckdownMeter
      if (Ctx.KnockdownMeter < Ctx.knockdownMax) {
         // Debug.Log("Regenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter += Time.deltaTime * 50;
      } else if (Ctx.KnockdownMeter > Ctx.knockdownMax){
         // Debug.Log("Degenerating: " + Ctx.KnockdownMeter);
         Ctx.KnockdownMeter = Ctx.knockdownMax;
      }
   }

   public override void ExitState() {
      // Debug.Log("ENEMY ROOT: EXITED MOVING");
      Ctx.Moving = false;
   }

   public override void CheckSwitchStates() {
      // Checking to see if the enemy got hurt
      if (Ctx.IsAttacked) {
         SwitchState(Factory.Hurt());
         return;
      }

      // Checking first if there is no player or the player is too far
      if (CurrentPlayerMachine == null) {
         SwitchState(Factory.Idle());
      } else {
         Vector3 vecToGoal = Ctx.gameObject.transform.position - Ctx.MovingGoal.position;
         if (Ctx.DontAttack) { return; } // If the enemy does not want to attack, he wont.
         if (Mathf.Abs(vecToGoal.z) > Ctx.zAttackDistance) { return; } // Enemy is too far on the z axis, so do not attack yet

         float dist = Vector3.Distance(Ctx.gameObject.transform.position, Ctx.MovingGoal.position);
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
      SetSubState(Factory.Chase());

      // if (Ctx.EnemyFlankType == EnemyStateMachine.FlankType.Boss) {
      //    Ctx.EnemyFlankDistanceGoal = Random.Range(7.3f, 11.7f);
      //    SetSubState(Factory.EnemyGuardState());
      // }else if (Ctx.EnemyFlankType == EnemyStateMachine.FlankType.Right) {
      //    Ctx.EnemyFlankDistanceGoal = Random.Range(7.3f, 11.7f);
      //    SetSubState(Factory.RightFlankState());
      // } else {
      //    Ctx.EnemyFlankDistanceGoal = Random.Range(7.3f, 11.7f);
      //    SetSubState(Factory.LeftFlankState());
      // }

      // Only state that should be set to the substate initially is the Stunned state
      // SetSubState(Factory.Stunned());
   }
}


// Enemy faces direction based off of the positivity of the x value and sidedness
      // Vector3 enemyScale = Ctx.transform.localScale;
      // if (dirFaceVec.x >= (.35f * sidedness)) {
      //    Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      // } else if (dirFaceVec.x <= (.35f * sidedness * -1)) {
      //    Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x), enemyScale.y, enemyScale.z);
      // }