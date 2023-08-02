using UnityEngine;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyLeftFlankState : EnemyBaseState
{
   // Variables
   private bool _madeToFlankGoal = false;

   public EnemyLeftFlankState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   public override void EnterState() {
      // Grabbing a spot to pursue the player
      if (!Ctx.CanPursue) {
         if (EnemyStateMachine.leftPursuingEnemies < EnemyStateMachine.leftPursuingMax) {
            Ctx.CanPursue = true;
            EnemyStateMachine.leftPursuingEnemies++;
         } else {
            Ctx.CanPursue = false;
         }
      }
      
      Ctx.CanPursue = true;
   }

   public override void UpdateState() {
      Ctx.MovingGoal = Ctx.CurrentPlayerMachine.transform;

      Vector3 vecToGoal = Ctx.gameObject.transform.position - Ctx.MovingGoal.position; // Player Position Offset
      float distanceToGoal = Vector3.Distance(Ctx.gameObject.transform.position, Ctx.MovingGoal.position); // Distance to the player

      if (vecToGoal.x < 0) {
         if (!_madeToFlankGoal) {
            // Left side, so we are just going directly for it
            Ctx.MovingGoalOffset = new Vector2(0, 0);
            Ctx.DontAttack = false;

            if (!Ctx.CanPursue) {
               Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal * -1, 0);
            }
         } else {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal * -1, 0);

            if ((vecToGoal.x < Ctx.distanceGoal * -1)  && (Ctx.CanPursue)) {
               _madeToFlankGoal = false;
               Ctx.DontAttack = false;
            } 
         }
      } else {
         Ctx.DontAttack = true;
         if (vecToGoal.z > 0) {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal * -1, 5);
         } else {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal * -1, -5);
         }
        _madeToFlankGoal = true;
      }

      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
   }

   public override void CheckSwitchStates() {

   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}

/*

OLD SCRIPT BELOW

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
      // Debug.Log("ENEMY SUB: ENTERED CHASE");
   }

   public override void UpdateState() {
      //needs functionality to alter the speed of enemy, right now its proportional to distance
      Vector3 directionToPlayer = CurrentPlayerMachine.gameObject.transform.position - Ctx.gameObject.transform.position;
      directionToPlayer = directionToPlayer.normalized * 10; //this value affects speed
      Ctx.Rigidbody.AddForce(directionToPlayer, ForceMode.Force);
      // Debug.Log(directionToPlayer.x);


      //make it so the right of enemy will always face player when chasing
      Vector3 enemyScale = Ctx.transform.localScale;
      if(directionToPlayer.x > 0) {
         Ctx.transform.localScale = new Vector3(Mathf.Abs(enemyScale.x),enemyScale.y,enemyScale.z);
      }else {
         Ctx.transform.localScale = new Vector3(-Mathf.Abs(enemyScale.x),enemyScale.y,enemyScale.z);
      }

      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
   }

   public override void CheckSwitchStates() {

   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}

*/