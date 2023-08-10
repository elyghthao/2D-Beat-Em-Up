using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Default substate of EnemyHurtState. When nothing is happening to the enemy other than recovering from having been
/// hit
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
   private NavMeshAgent agent;

   public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) {
   
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
        Ctx.hasAgent = true;

        Vector3 newPos = Ctx.gameObject.transform.position;
        newPos.y += 3;

        newObj.transform.position = newPos;
   }

   public override void EnterState() {
      if (!Ctx.AgentObject) {
         CreateFakeAI();
      }
      Ctx.AgentObject.transform.position = Ctx.gameObject.transform.position;
      agent = Ctx.AgentObject.GetComponent<NavMeshAgent>();
   }

   public override void UpdateState() {
      Vector3 goalPos = Ctx.CurrentPlayerMachine.transform.position;
      goalPos.x += 2;

      agent.SetDestination(goalPos);
      Ctx.MovingGoalOffset = new Vector2(0,0);

      Vector3 newPos = Ctx.AgentObject.transform.position;
      newPos.y = Ctx.gameObject.transform.position.y;

      Ctx.gameObject.transform.position = newPos;
   }

   public override void FixedUpdateState() {

   }

   public override void ExitState() {
      // Debug.Log("ENEMY SUB: EXITED CHASE");
      if (Ctx.hasAgent) {
         Ctx.hasAgent = false;
      }
   }

   public override void CheckSwitchStates() {
      // Determines when to start going to the left or right
      if (!Ctx.UseChaseAI()) {
         if (Ctx.EnemyFlankType == EnemyStateMachine.FlankType.Right) {
            Ctx.EnemyFlankDistanceGoal = Random.Range(7.3f, 11.7f);
            SwitchState(Factory.RightFlankState());
         } else {
            Ctx.EnemyFlankDistanceGoal = Random.Range(7.3f, 11.7f);
            SwitchState(Factory.LeftFlankState());
         }
      } 
   }

   public override void InitializeSubState() {
      throw new System.NotImplementedException();
   }
}