using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyRightFlankState : EnemyBaseState
{
   // Variables
   private bool _madeToFlankGoal = false;
   private NavMeshAgent agent;

   public EnemyRightFlankState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   private void CreateFakeAI() {
        GameObject newObj = new GameObject("Fake_AI");
        newObj.AddComponent<NavMeshAgent>();
        newObj.layer = LayerMask.NameToLayer("Enemy");

        agent = newObj.GetComponent<NavMeshAgent>();
        agent.speed = Ctx.movementSpeed;
        agent.angularSpeed = 90000;
        agent.acceleration = 90000;

        agent.radius = 0.7f;
        agent.height = 3.85f;

        Ctx.AgentObject = newObj;
        Ctx.HasAgent = true;

        Vector3 newPos = Ctx.gameObject.transform.position;
        newPos.y += 3;

        newObj.transform.position = newPos;
   }

   public override void EnterState() {
      // Grabbing a spot to pursue the player
      if (!Ctx.CanPursue) {
         if (EnemyStateMachine.rightPursuingEnemies < EnemyStateMachine.rightPursuingMax) {
            Ctx.CanPursue = true;
            EnemyStateMachine.rightPursuingEnemies++;
         } else {
            Ctx.CanPursue = false;
         }
      }
      
      Ctx.CanPursue = true;

      if (!Ctx.AgentObject) {
         CreateFakeAI();
      }
      Ctx.AgentObject.transform.position = Ctx.gameObject.transform.position;
      agent = Ctx.AgentObject.GetComponent<NavMeshAgent>();
   }

   public override void UpdateState() {
      Ctx.MovingGoal = Ctx.CurrentPlayerMachine.transform;
      
      Vector3 vecToGoal = Ctx.gameObject.transform.position - Ctx.MovingGoal.position; // Player Position Offset
      float distanceToGoal = Vector3.Distance(Ctx.gameObject.transform.position, Ctx.MovingGoal.position); // Distance to the player

      if (vecToGoal.x > 0) {
         if (!_madeToFlankGoal) {
            // Right side, so we are just going directly for it
            Ctx.MovingGoalOffset = new Vector2(0, 0);
            Ctx.DontAttack = false;
            if (!Ctx.CanPursue) {
               Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, 0);
            }
         } else {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, 0);
            if (vecToGoal.x > Ctx.distanceGoal && Ctx.CanPursue) {
               _madeToFlankGoal = false;
               Ctx.DontAttack = false;
            } 
         }
      } else {
         Ctx.DontAttack = true;
         if (vecToGoal.z > 0) {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, 5);
         } else {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, -5);
         }
         _madeToFlankGoal = true;
      }

      // Chasing the fake AI
      Vector3 realGoal = Ctx.MovingGoal.position;
      realGoal.x += Ctx.MovingGoalOffset.x;
      realGoal.y = Ctx.gameObject.transform.position.y;
      realGoal.z += Ctx.MovingGoalOffset.y;

      if (((distanceToGoal > Ctx.distanceGoal) || Ctx.DontAttack)){
         Ctx.inPosition = false; 
         Ctx.Rigidbody.AddForce(vecToGoal, ForceMode.Force);
      } else {
         Ctx.inPosition = true;
         realGoal.x = Ctx.CurrentPlayerMachine.transform.position.x + 2.3f;
         realGoal.y = Ctx.gameObject.transform.position.y;
         realGoal.z = Ctx.CurrentPlayerMachine.transform.position.z + Random.Range(-.7f, .7f);
      }

      agent.SetDestination(realGoal);

      Vector3 newPos = Ctx.AgentObject.transform.position;
      newPos.y = Ctx.gameObject.transform.position.y;

      Ctx.gameObject.transform.position = newPos;
   }

   public override void FixedUpdateState() {

   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
      Ctx.HasAgent = false;
   }

   public override void CheckSwitchStates() {

   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}

/*

using UnityEngine;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyRightFlankState : EnemyBaseState
{
   // Variables
   private bool _madeToFlankGoal = false;

   public EnemyRightFlankState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
   }

   public override void EnterState() {
      // Grabbing a spot to pursue the player
      if (!Ctx.CanPursue) {
         if (EnemyStateMachine.rightPursuingEnemies < EnemyStateMachine.rightPursuingMax) {
            Ctx.CanPursue = true;
            EnemyStateMachine.rightPursuingEnemies++;
         } else {
            Ctx.CanPursue = false;
         }
      }
      
      Ctx.CanPursue = true;
   }

   public override void UpdateState() {
      
   }

   public override void FixedUpdateState() {
      Ctx.MovingGoal = Ctx.CurrentPlayerMachine.transform;

      Vector3 vecToGoal = Ctx.gameObject.transform.position - Ctx.MovingGoal.position; // Player Position Offset
      float distanceToGoal = Vector3.Distance(Ctx.gameObject.transform.position, Ctx.MovingGoal.position); // Distance to the player

      if (vecToGoal.x > 0) {
         if (!_madeToFlankGoal) {
            // Right side, so we are just going directly for it
            Ctx.MovingGoalOffset = new Vector2(0, 0);
            Ctx.DontAttack = false;

            if (!Ctx.CanPursue) {
               Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, 0);
            }
         } else {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, 0);

            if (vecToGoal.x > Ctx.distanceGoal && Ctx.CanPursue) {
               _madeToFlankGoal = false;
               Ctx.DontAttack = false;
            } 
         }
      } else {
         Ctx.DontAttack = true;
         if (vecToGoal.z > 0) {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, 5);
         } else {
            Ctx.MovingGoalOffset = new Vector2(Ctx.EnemyFlankDistanceGoal, -5);
         }
         _madeToFlankGoal = true;
      }

      CheckSwitchStates();
   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
   }

   public override void CheckSwitchStates() {
      if (Ctx.UseChaseAI()) {
         SwitchState(Factory.Chase());
      }
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}

*/