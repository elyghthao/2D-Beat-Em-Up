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
      //   newObj.AddComponent<Rigidbody>();
      //   newObj.AddComponent<CapsuleCollider>();
        newObj.layer = LayerMask.NameToLayer("Enemy");

        agent = newObj.GetComponent<NavMeshAgent>();
        agent.speed = Ctx.movementSpeed;
        agent.angularSpeed = 90000;
        agent.acceleration = 90000;
        agent.radius = 0.7f;
        agent.height = 3.85f;

      //   Rigidbody body = newObj.GetComponent<Rigidbody>();
      //   body.interpolation = RigidbodyInterpolation.Interpolate;
      //   body.collisionDetectionMode = CollisionDetectionMode.Continuous;
      //   body.isKinematic = true;

      //   CapsuleCollider collid = newObj.GetComponent<CapsuleCollider>();
      //   collid.radius = 0.7f;
      //   collid.height = 3.85f;
      //   collid.center = new Vector3(0, 2.05f, 0);

        Ctx.AgentObject = newObj;
        Ctx.RealAgent = agent;

      //   newObj.transform.position = newPos;
   }

   public override void EnterState() {
      if (!Ctx.AgentObject) {
         CreateFakeAI();
      }
      Ctx.AgentObject.transform.position = Ctx.gameObject.transform.position;
      agent = Ctx.AgentObject.GetComponent<NavMeshAgent>();
      Ctx.RealAgent = agent;

      Vector3 newPos = Ctx.gameObject.transform.position;
      newPos.y += 3.5f;
      agent.Warp(newPos);

      Ctx.HasAgent = true;
   }

   public override void UpdateState() {
      float newGoalDist = Ctx.attackDistance - .4f;

      if (Ctx.EnemyFlankType == EnemyStateMachine.FlankType.Left) {
         Ctx.MovingGoalOffset = new Vector2(newGoalDist * -1, 0);
      } else {
         Ctx.MovingGoalOffset = new Vector2(newGoalDist, 0);
      }

      // Vector3 goalPos = Ctx.CurrentPlayerMachine.transform.position;
      // goalPos.x += 2;

      // agent.SetDestination(goalPos);
      // Ctx.MovingGoalOffset = new Vector2(0,0);

      // Vector3 newPos = Ctx.AgentObject.transform.position;
      // newPos.y = Ctx.gameObject.transform.position.y;

      // Ctx.gameObject.transform.position = newPos;
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